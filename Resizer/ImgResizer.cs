using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Resizer
{
    public class ImgResizer
    {
        public static void ResizeImagesFromDirectory()
        {

            var docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string directory = $@"{docPath}/Images/";

            Directory.CreateDirectory($@"{docPath}\Converted_Images");

            Console.WriteLine("Provide img extension: BMP (.bmp),GIF (.gif), JPEG (.jpg), PNG (.png)");

            var extension = Console.ReadLine();

            extension = Validator.Validate(extension);

            Console.WriteLine($"Loading images with {extension} extension...");

            var images = Directory.GetFiles(directory, $"*{extension}",
                                             SearchOption.AllDirectories)
                                   .Select(Image.FromFile).ToList();
            if (images.Any())
            {
                Console.WriteLine($"Found {images.Count()} images with {extension} extension");
                Console.WriteLine("Provie width in pixels:");
                var widthString = Console.ReadLine();
                Console.WriteLine("Provide height in pixels:");
                var heightString = Console.ReadLine();



                int width = Parser.ParseToInt(widthString);
                int height = Parser.ParseToInt(heightString);

                var descRect = new Rectangle(0, 0, width, height);
                var output = new Bitmap(width, height);
                int i = 0;


                foreach (var img in images)
                {

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
                    Console.WriteLine($"Converting to {width}x{height}.");
                    output.Save($@"{docPath}\ConvertedImages\converted_img{i}{extension}");
                    i++;

                    Console.WriteLine($@"Saving to {docPath}\ConvertedImages\converted_img{i}{extension}");
                }
            }
            else
            {
                Console.WriteLine($"No files with extension {extension} was found in directory.");
            }

        }
    }
}
