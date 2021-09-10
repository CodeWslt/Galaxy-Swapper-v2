﻿using System.IO;
using System.Runtime.CompilerServices;
using CUE4Parse.Compression;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.FileProvider
{
    public class OsGameFile : GameFile
    {
        public readonly FileInfo ActualFile;
        public OsGameFile(DirectoryInfo baseDir, FileInfo info, EGame game, UE4Version ver)
            : base(info.FullName.Substring(baseDir.FullName.Length + 1).Replace('\\', '/'), info.Length, game, ver)
        {
            ActualFile = info;
        }
        
        public override bool IsEncrypted => false;
        public override CompressionMethod CompressionMethod => CompressionMethod.None;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte[] Read() => File.ReadAllBytes(ActualFile.FullName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override FArchive CreateReader() => new FByteArchive(Path, Read());
    }
}