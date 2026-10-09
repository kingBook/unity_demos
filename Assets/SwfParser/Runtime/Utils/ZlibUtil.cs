using System.IO;
using Unity.SharpZipLib.Zip.Compression.Streams;

namespace SwfParserRuntime {

    public static class ZlibUtil {

        /// <summary>
        /// 复制流
        /// </summary>
        /// <param name="input">原始流</param>
        /// <param name="output">目标流</param>
        public static void CopyStream(System.IO.Stream input, System.IO.Stream output) {
            byte[] buffer = new byte[2000];
            int len;
            while ((len = input.Read(buffer, 0, 2000)) > 0) {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }

        // /// <summary>
        // /// 压缩字节数组
        // /// </summary>
        // /// <param name="sourceByte">需要被压缩的字节数组</param>
        // /// <returns>压缩后的字节数组</returns>
        // public static byte[] CompressBytes(byte[] sourceByte) {
        //     MemoryStream inputStream = new(sourceByte);
        //     Stream outStream = CompressStream(inputStream);
        //     byte[] outPutByteArray = new byte[outStream.Length];
        //     outStream.Position = 0;
        //     outStream.Read(outPutByteArray, 0, outPutByteArray.Length);
        //     outStream.Close();
        //     inputStream.Close();
        //     return outPutByteArray;
        // }

        /// <summary>
        /// 解压缩字节数组
        /// </summary>
        /// <param name="sourceByte">需要被解压缩的字节数组</param>
        /// <returns>解压后的字节数组</returns>
        public static byte[] DeCompressBytes(byte[] sourceByte) {
            MemoryStream inputStream = new(sourceByte);
            Stream outputStream = DecompressStream(inputStream);
            byte[] outputBytes = new byte[outputStream.Length];
            outputStream.Position = 0;
            outputStream.Read(outputBytes, 0, outputBytes.Length);
            outputStream.Close();
            inputStream.Close();
            return outputBytes;
        }

        // /// <summary>
        // /// zlib 压缩(此方法还未验证)
        // /// </summary>
        // /// <param name="sourceStream">需要被压缩的流</param>
        // /// <returns>压缩后的流</returns>
        // public static MemoryStream CompressStream(Stream sourceStream) {
        //     MemoryStream streamOut = new();

        //     // old
        //     // ZOutputStream streamZOut = new ZOutputStream(streamOut, zlibConst.Z_DEFAULT_COMPRESSION);
        //     // CopyStream(sourceStream, streamZOut);
        //     // streamZOut.finish();
        //     // return streamOut;

        //     DeflaterOutputStream zOut = new(streamOut);
        //     sourceStream.CopyTo(zOut);
        //     zOut.Finish();
        //     //zOut.Flush();
        //     return streamOut;
        // }

        /// <summary>
        /// zlib 解压
        /// </summary>
        /// <param name="sourceStream">需要被解压缩的流</param>
        /// <returns>解压后的流</returns>
        public static MemoryStream DecompressStream(Stream sourceStream) {
            MemoryStream outStream = new();

            // old
            // ZOutputStream outZStream = new ZOutputStream(outStream);
            // CopyStream(sourceStream, outZStream);
            // outZStream.finish();
            // return outStream;

            InflaterInputStream zIn = new(sourceStream);
            zIn.CopyTo(outStream);
            return outStream;
        }
    }
}