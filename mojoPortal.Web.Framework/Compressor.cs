using System.IO;
using System.IO.Compression;


namespace mojoPortal.Web.Framework
{
    /// <summary>
    /// http://www.codeproject.com/KB/viewstate/ViewStateCompression.aspx
    /// </summary>
    public static class Compressor
    {
        
            public static byte[] Compress(byte[] data)
            {
                using (MemoryStream output = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(output, CompressionMode.Compress, true))
                    {
                        gzip.Write(data, 0, data.Length);
                        gzip.Close();
                        return output.ToArray();
                    }
                }
            }

            public static byte[] Decompress(byte[] data)
            {
                using (MemoryStream input = new MemoryStream())
                {
                    input.Write(data, 0, data.Length);
                    input.Position = 0;
                    using (GZipStream gzip = new GZipStream(input, CompressionMode.Decompress, true))
                    {
                        using (MemoryStream output = new MemoryStream())
                        {
                            byte[] buff = new byte[64];
                            int read = -1;
                            read = gzip.Read(buff, 0, buff.Length);
                            while (read > 0)
                            {
                                output.Write(buff, 0, read);
                                read = gzip.Read(buff, 0, buff.Length);
                            }
                            gzip.Close();
                            return output.ToArray();
                        }
                    }
                }
            }
        

        //public static byte[] Compress(byte[] data)
        //{
        //    MemoryStream output = new MemoryStream();
        //    GZipStream gzip = new GZipStream(output,
        //                      CompressionMode.Compress, true);
        //    gzip.Write(data, 0, data.Length);
        //    gzip.Close();
        //    return output.ToArray();
        //}

        //public static byte[] Decompress(byte[] data)
        //{
        //    MemoryStream input = new MemoryStream();
        //    input.Write(data, 0, data.Length);
        //    input.Position = 0;
        //    GZipStream gzip = new GZipStream(input,
        //                      CompressionMode.Decompress, true);
        //    MemoryStream output = new MemoryStream();
        //    byte[] buff = new byte[64];
        //    int read = -1;
        //    read = gzip.Read(buff, 0, buff.Length);
        //    while (read > 0)
        //    {
        //        output.Write(buff, 0, read);
        //        read = gzip.Read(buff, 0, buff.Length);
        //    }
        //    gzip.Close();
        //    return output.ToArray();
        //}
    }
}
