using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Galaxy_Swapper_v2.Workspace.Encryption
{
    public static class Compression
    {
        public static string Compress(string uncompressedString)
        {
            byte[] compressedBytes;

            using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString)))
            {
                using (var compressedStream = new MemoryStream())
                {
                    using (var compressorStream = new DeflateStream(compressedStream, CompressionLevel.Optimal, true))
                    {
                        uncompressedStream.CopyTo(compressorStream);
                    }
                    compressedBytes = compressedStream.ToArray();
                }
            }
            return Encryption.Base64Encryption.Encrypt(Convert.ToBase64String(compressedBytes));
        }
        public static string Decompress(this string compressedString)
        {
            compressedString = Encryption.Base64Encryption.Decrypt(compressedString);
            byte[] decompressedBytes;
            var compressedStream = new MemoryStream(Convert.FromBase64String(compressedString));
            using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
            {
                using (var decompressedStream = new MemoryStream())
                {
                    decompressorStream.CopyTo(decompressedStream);

                    decompressedBytes = decompressedStream.ToArray();
                }
            }
            return Encoding.UTF8.GetString(decompressedBytes);
        }
    }
}
