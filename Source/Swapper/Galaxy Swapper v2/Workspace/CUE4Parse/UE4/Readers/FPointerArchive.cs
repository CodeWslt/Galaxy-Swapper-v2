﻿using System;
using System.IO;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Readers
{
    public class FPointerArchive : FArchive
    {
        private readonly unsafe byte* _ptr;

        public unsafe FPointerArchive(string name, byte* ptr, long length, EGame game = EGame.GAME_UE4_LATEST, UE4Version ver = UE4Version.VER_UE4_DETERMINE_BY_GAME)
            : base(game, ver)
        {
            _ptr = ptr;
            Name = name;
            Length = length;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int Read(byte[] buffer, int offset, int count)
        {
            unsafe
            {
                Unsafe.CopyBlockUnaligned(ref buffer[offset], ref _ptr[Position], (uint) count);
                Position += count;
                return count;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override long Seek(long offset, SeekOrigin origin)
        {
            Position = origin switch
            {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => Position + offset,
                SeekOrigin.End => Length + offset,
                _ => throw new ArgumentOutOfRangeException()
            };
            return Position;
        }

        public override bool CanSeek { get; } = true;
        public override long Length { get; }
        public override long Position { get; set; }
        public override string Name { get; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override T Read<T>()
        {
            unsafe
            {
                var size = Unsafe.SizeOf<T>();
                var result = Unsafe.ReadUnaligned<T>(ref _ptr[Position]);
                Position += size;
                return result;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte[] ReadBytes(int length)
        {
            var buffer = new byte[length];
            Read(buffer, 0, length);
            return buffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Serialize(byte* ptr, int length)
        {
            Unsafe.CopyBlockUnaligned(ref ptr[0], ref _ptr[Position], (uint) length);
            Position += length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override T[] ReadArray<T>(int length)
        {
            unsafe
            {
                var size = length * Unsafe.SizeOf<T>();
                var result = new T[length];
                Unsafe.CopyBlockUnaligned(ref Unsafe.As<T, byte>(ref result[0]), ref _ptr[Position], (uint) size);
                Position += size;
                return result;
            }
        }

        public override object Clone()
        {
            unsafe
            {
                return new FPointerArchive(Name, _ptr, Length, Game, Ver) {Position = Position};
            }
        }
    }
}