using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Layer.Helpers
{
    public static class Helper
    {
        public static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

        public static byte[] ImageToByte(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public static MemoryStream ToMemoryStream(string path)
        {
            using (FileStream hondaFilestream = File.OpenRead(path))
            {
                using (MemoryStream hondaMemoryStream = new MemoryStream())
                {
                    hondaFilestream.CopyTo(hondaMemoryStream);

                    return hondaMemoryStream;
                }
            }
        }

        public static IFormFile ToFormFile(string name, string path)
        {
            byte[] bytesValue = Encoding.UTF8.GetBytes(path);

            return new FormFile(
                baseStream: new MemoryStream(bytesValue),
                baseStreamOffset: 0,
                length: bytesValue.Length,
                name: "Data",
                fileName: name
            );
        }

        public static async Task<byte[]> ToBytes(this IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);

            return memoryStream.ToArray();
        }

        public static byte[] ToFileBytes(this string filePath)
        {
            return System.IO.File.ReadAllBytes(filePath);
        }

        public static async Task<MemoryStream> ToMemoryStream(this IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);

            return memoryStream;
        }

        public static byte[] ToFileBytes(this IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }

        public static void SaveFile(string path, MemoryStream memoryStream)
        {
            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                memoryStream.WriteTo(file);
            }
        }

        public static void SaveFile(string path, byte[] fileBytes)
        {
            using (Stream file = File.OpenWrite(path))
            {
                file.Write(fileBytes, 0, fileBytes.Length);
            }
        }
    }
}
