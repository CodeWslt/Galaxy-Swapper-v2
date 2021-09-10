using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Galaxy_Swapper_v2.Workspace.Other
{
    public static class Oodle
    {
        [DllImport("oo2core_6_win64.dll")]
        private static extern int OodleLZ_Compress(OodleType format, byte[] UnCompressedBytes, long ByteLength, byte[] outputCompressedBytes, OodleLevel level, uint unused1, uint unused2, uint unused3);
        [DllImport("oo2core_6_win64.dll")]
        private static extern int OodleLZ_Decompress(byte[] buffer, long bufferSize, byte[] outputBuffer, long outputBufferSize, uint a, uint b, ulong c, uint d, uint e, uint f, uint g, uint h, uint i, uint threadModule);
        public static byte[] Compress(byte[] UnCompressedBytes)
        {
            if (!File.Exists("oo2core_6_win64.dll"))
            {
                using (WebClient w = new WebClient())
                    w.DownloadFile("https://cdn.discordapp.com/attachments/845326712338382881/882819204343017572/oo2core_6_win64.dll", "oo2core_6_win64.dll");
                Thread.Sleep(1000);
            }
            uint compressedBufferSize = GetCompressionBound((uint)UnCompressedBytes.Length);

            byte[] compressedBuffer = new byte[compressedBufferSize];
            int compressedCount = OodleLZ_Compress(OodleType.Kraken, UnCompressedBytes, UnCompressedBytes.Length, compressedBuffer, OodleLevel.Level5, 0, 0, 0);
            byte[] outputBuffer = new byte[compressedCount];
            Buffer.BlockCopy(compressedBuffer, 0, outputBuffer, 0, compressedCount);
            return outputBuffer;
        }
        public static byte[] Decompress(byte[] CompressedBytes, int ByteLength, int uncompressedSize)
        {
            byte[] decompressedBuffer = new byte[uncompressedSize];
            int decompressedCount = OodleLZ_Decompress(CompressedBytes, ByteLength, decompressedBuffer, uncompressedSize, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
            if (decompressedCount == uncompressedSize)
            {
                return decompressedBuffer;
            }
            return decompressedBuffer.Take(decompressedCount).ToArray();
        }
        private static uint GetCompressionBound(uint bufferSize)
        {
            return bufferSize + 274 * ((bufferSize + 0x3FFFF) / 0x40000);
        }
        public static int GetDecompressionBound(int bufferSize)
        {
            int v2 = bufferSize + 272 + 0;
            if (bufferSize + 16731 + 2 * (bufferSize + 0x3FFFF) / 0x40000 < v2)
            {
                v2 = bufferSize + 16731 + 2 * (bufferSize + 0x3FFFF) / 0x40000;
            }
            return v2;
        }
        public enum OodleLevel : ulong
        {
            None,
            Fastest,
            Faster,
            Fast,
            Normal,
            Level1,
            Level2,
            Level3,
            Level4,
            Level5
        }
        public enum OodleType : uint
        {
            LZH,
            LZHLW,
            LZNIB,
            None,
            LZB16,
            LZBLW,
            LZA,
            LZNA,
            Kraken,
            Mermaid,
            BitKnit,
            Selkie,
            Akkorokamui
        }
    }
}