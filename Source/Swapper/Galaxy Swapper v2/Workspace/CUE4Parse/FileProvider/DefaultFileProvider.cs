﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CUE4Parse.FileProvider.Vfs;
using CUE4Parse.UE4.IO;
using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.UE4.Pak;
using CUE4Parse.UE4.Versions;
using CUE4Parse.Utils;

namespace CUE4Parse.FileProvider
{
    public class DefaultFileProvider : AbstractVfsFileProvider
    {
        private DirectoryInfo _workingDirectory;
        private readonly SearchOption _searchOption;
        private readonly List<DirectoryInfo> _extraDirectories;

        public DefaultFileProvider(string directory, SearchOption searchOption, bool caseSensitive = false,
            EGame game = EGame.GAME_UE4_LATEST, UE4Version ver = UE4Version.VER_UE4_DETERMINE_BY_GAME)
            : this(new DirectoryInfo(directory), searchOption, caseSensitive, game, ver)
        {
        }

        public DefaultFileProvider(DirectoryInfo directory, SearchOption searchOption, bool caseSensitive = false,
            EGame game = EGame.GAME_UE4_LATEST, UE4Version ver = UE4Version.VER_UE4_DETERMINE_BY_GAME) : base(caseSensitive, game, ver)
        {
            _workingDirectory = directory;
            _searchOption = searchOption;
        }

        public DefaultFileProvider(DirectoryInfo mainDirectory, List<DirectoryInfo> extraDirectories, SearchOption searchOption, bool caseSensitive = false,
            EGame game = EGame.GAME_UE4_LATEST, UE4Version ver = UE4Version.VER_UE4_DETERMINE_BY_GAME) : base(caseSensitive, game, ver)
        {
            _workingDirectory = mainDirectory;
            _extraDirectories = extraDirectories;
            _searchOption = searchOption;
        }

        public void Initialize()
        {
            if (!_workingDirectory.Exists) throw new ArgumentException("Given directory must exist", nameof(_workingDirectory));

            var availableFiles = new List<Dictionary<string, GameFile>> { IterateFiles(_workingDirectory, _searchOption) };

            if (_extraDirectories is { Count: > 0 })
            {
                availableFiles.AddRange(_extraDirectories.Select(directory => IterateFiles(directory, _searchOption)));
            }

            foreach (var osFiles in availableFiles)
            {
                _files.AddFiles(osFiles);
            }
        }

        //Load one file
        public void Initialize(string containsfilename)
        {
            if (!_workingDirectory.Exists) throw new ArgumentException("Given directory must exist", nameof(_workingDirectory));

            var availableFiles = new List<Dictionary<string, GameFile>> { SpecificFiles(containsfilename, _workingDirectory, _searchOption) };

            if (_extraDirectories is { Count: > 0 })
            {
                availableFiles.AddRange(_extraDirectories.Select(directory => SpecificFiles(containsfilename, directory, _searchOption)));
            }

            foreach (var osFiles in availableFiles)
            {
                _files.AddFiles(osFiles);
            }
        }





        private Dictionary<string, GameFile> SpecificFiles(string containsfilename, DirectoryInfo directory, SearchOption option)
        {
            var osFiles = new Dictionary<string, GameFile>();
            foreach (var file in directory.EnumerateFiles("*.*", option))
            {
                if (!file.Name.Contains(containsfilename))
                    continue;

                var ext = file.Extension.SubstringAfter('.');
                if (!file.Exists || string.IsNullOrEmpty(ext)) continue;
                if (ext.Equals("pak", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var reader = new PakFileReader(file, Game, Ver) { IsConcurrent = true };
                        if (reader.IsEncrypted && !_requiredKeys.ContainsKey(reader.Info.EncryptionKeyGuid))
                        {
                            _requiredKeys[reader.Info.EncryptionKeyGuid] = null;
                        }

                        _unloadedVfs[reader] = null;
                    }
                    catch (Exception e)
                    {
                        //Log.Warning(e.ToString());
                    }
                }
                else if (ext.Equals("utoc", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var reader = new IoStoreReader(file, EIoStoreTocReadOptions.ReadDirectoryIndex, Game, Ver) { IsConcurrent = true };
                        if (reader.IsEncrypted && !_requiredKeys.ContainsKey(reader.Info.EncryptionKeyGuid))
                        {
                            _requiredKeys[reader.Info.EncryptionKeyGuid] = null;
                        }

                        _unloadedVfs[reader] = null;
                    }
                    catch (Exception e)
                    {
                        // Log.Warning(e.ToString());
                    }
                }
                else
                {
                    // Register local file only if it has a known extension, we don't need every file
                    if (!GameFile.Ue4KnownExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase)) continue;

                    var osFile = new OsGameFile(_workingDirectory, file, Game, Ver);
                    if (IsCaseInsensitive) osFiles[osFile.Path.ToLowerInvariant()] = osFile;
                    else osFiles[osFile.Path] = osFile;
                }
            }

            return osFiles;
        }


        private Dictionary<string, GameFile> IterateFiles(DirectoryInfo directory, SearchOption option)
        {
            var osFiles = new Dictionary<string, GameFile>();

            foreach (var file in directory.EnumerateFiles("*.*", option))
            {
                var ext = file.Extension.SubstringAfter('.');
                if (!file.Exists || string.IsNullOrEmpty(ext)) continue;
                if (ext.Equals("pak", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var reader = new PakFileReader(file, Game, Ver) { IsConcurrent = true };
                        if (reader.IsEncrypted && !_requiredKeys.ContainsKey(reader.Info.EncryptionKeyGuid))
                        {
                            _requiredKeys[reader.Info.EncryptionKeyGuid] = null;
                        }

                        _unloadedVfs[reader] = null;
                    }
                    catch (Exception e)
                    {
                        //Log.Warning(e.ToString());
                    }
                }
                else if (ext.Equals("utoc", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var reader = new IoStoreReader(file, EIoStoreTocReadOptions.ReadDirectoryIndex, Game, Ver) { IsConcurrent = true };
                        if (reader.IsEncrypted && !_requiredKeys.ContainsKey(reader.Info.EncryptionKeyGuid))
                        {
                            _requiredKeys[reader.Info.EncryptionKeyGuid] = null;
                        }

                        _unloadedVfs[reader] = null;
                    }
                    catch (Exception e)
                    {
                        //Log.Warning(e.ToString());
                    }
                }
                else
                {
                    // Register local file only if it has a known extension, we don't need every file
                    if (!GameFile.Ue4KnownExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase)) continue;

                    var osFile = new OsGameFile(_workingDirectory, file, Game, Ver);
                    if (IsCaseInsensitive) osFiles[osFile.Path.ToLowerInvariant()] = osFile;
                    else osFiles[osFile.Path] = osFile;
                }
            }

            return osFiles;
        }
    }
}