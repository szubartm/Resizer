using Microsoft.AspNetCore.Mvc;
using Resizer;
using ResizerWEB.FileUpload;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;

namespace ResizerWEB.ImageUpload
{
    public class ResizeImageService : IResizeImageService
    {
        
        public void ResizeImage(List<IFormFile> images, string widthString, string heightString)
        {
            var width = Parser.ParseToInt(widthString);
            var height = Parser.ParseToInt(heightString);
            var descRect = new Rectangle(0, 0, 0, 0);
            var output = new Bitmap(100, 100);
            int i = 0;
            string extension = ".jpg";
            foreach (var image in images)
            {

                var type = image.ContentType;


                using (var memoryStream = new MemoryStream())
                {
                    image.CopyTo(memoryStream);
                    using (var img = Image.FromStream(memoryStream))
                    {
                        

                        if (img.Width < img.Height)
                        {
                            descRect.Width = height;
                            descRect.Height = width;
                            output = new Bitmap(height, width);
                            Console.WriteLine("Detected horizontal image.");
                            Console.WriteLine($"Converting to {height}x{width}.");
                        }
                        else
                        {
                            descRect = new Rectangle(0, 0, width, height);
                            output = new Bitmap(width, height);
                            Console.WriteLine($"Converting to {width}x{height}.");
                        }



                        output.SetResolution(img.HorizontalResolution, img.VerticalResolution);

                        using (var gr = Graphics.FromImage(output))
                        {
                            gr.CompositingMode = CompositingMode.SourceCopy;
                            gr.CompositingQuality = CompositingQuality.HighQuality;
                            gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            gr.SmoothingMode = SmoothingMode.HighQuality;
                            gr.PixelOffsetMode = PixelOffsetMode.HighQuality;

                            using (var wrapMode = new ImageAttributes())
                            {
                                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                                gr.DrawImage(img, descRect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, wrapMode);
                            }
                        }

                        
                        img.Dispose();

                        output.Save($"wwwroot\\converted\\converted{i}.png");
                        i++;
                    }
                }
            }




           
        }

        public byte[] Zip()
        {
            string[] filepaths = Directory.GetFiles("wwwroot\\converted\\");
            List<FileModel> files = new List<FileModel>();
            foreach (var filepath in filepaths)
            {
                files.Add(new FileModel { Name = Path.GetFileName(filepath) });
            }
            string output = "wwwroot\\converted\\ConvertedImages.zip";
            using (ZipOutputStream outputStream = new ZipOutputStream(System.IO.File.Create(output)))
            {
                outputStream.SetLevel(9);
                byte[] buffer = new byte[4096];
                var ImageList = new List<string>();
                foreach (var file in files)
                {
                    string path = $"wwwroot\\converted\\{file.Name}";
                    ImageList.Add(path);


                }

                foreach (var image in ImageList)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(image));
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    outputStream.PutNextEntry(entry);

                    using (FileStream fileStream = System.IO.File.OpenRead(image))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fileStream.Read(buffer, 0, buffer.Length);
                            outputStream.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                    System.IO.File.Delete(image);
                }

                outputStream.Finish();
                outputStream.Flush();
                outputStream.Close();
            }
            byte[] finalResult = System.IO.File.ReadAllBytes(output);
            if (System.IO.File.Exists(output))
            {
                System.IO.File.Delete(output);
            }

            if (finalResult == null || !finalResult.Any())
            {
                throw new Exception(String.Format("Not Found"));
            }
            return finalResult;
        }
    }
    
    
}
