﻿using System;
using System.IO;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Readers
{
    public class FStreamArchive : FArchive
    {
        private readonly Stream _baseStream;

        public FStreamArchive(string name, Stream baseStream, EGame game = EGame.GAME_UE4_LATEST, UE4Version ver = UE4Version.VER_UE4_DETERMINE_BY_GAME)
            : base(game, ver)
        {
            _baseStream = baseStream;
            Name = name;
        }

        public override void Close() => _baseStream.Close();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int Read(byte[] buffer, int offset, int count)
            => _baseStream.Read(buffer, offset, count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override long Seek(long offset, SeekOrigin origin)
            => _baseStream.Seek(offset, origin);

        public override bool CanSeek => _baseStream.CanSeek;
        public override long Length => _baseStream.Length;
        public override long Position
        {
            get => _baseStream.Position;
            set => _baseStream.Position = value;
        }

        public override string Name { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte[] ReadBytes(int length)
        {
            var result = new byte[length];
            _baseStream.Read(result, 0, length);
            return result;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Serialize(byte* ptr, int length)
        {
            var bytes = ReadBytes(length);
            Unsafe.CopyBlockUnaligned(ref ptr[0], ref bytes[0], (uint) length);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override T Read<T>()
        {
            var size = Unsafe.SizeOf<T>();
            var buffer = ReadBytes(size);
            return Unsafe.ReadUnaligned<T>(ref buffer[0]);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override T[] ReadArray<T>(int length)
        {
            var size = Unsafe.SizeOf<T>();
            var buffer = ReadBytes(size * length);
            var result = new T[length];
            Unsafe.CopyBlockUnaligned(ref Unsafe.As<T, byte>(ref result[0]), ref buffer[0], (uint)(length * size));
            return result;
        }

        public override object Clone()
        {
            return _baseStream switch
            {
                ICloneable cloneable => new FStreamArchive(Name, (Stream) cloneable.Clone(), Game, Ver) {Position = Position},
                FileStream fileStream => new FStreamArchive(Name, File.Open(fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Game, Ver) {Position = Position},
                _ => new FStreamArchive(Name, _baseStream, Game, Ver) {Position = Position}
            };
        }
    }
}