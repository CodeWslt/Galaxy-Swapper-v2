using Ionic.Zlib;
using System.IO;

namespace Galaxy_Swapper_v2.Workspace.Other
{
    public static class Zlib
    {
        public static byte[] ZlibCompress(byte[] input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (ZlibStream zls = new ZlibStream(ms, CompressionMode.Compress, CompressionLevel.Level7))
                    zls.Write(input, 0, input.Length);

                return ms.ToArray();
            }
        }
    }
}
