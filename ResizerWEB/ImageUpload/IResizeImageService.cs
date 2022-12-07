using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Security.Cryptography;

namespace ResizerWEB.FileUpload
{
    public interface IResizeImageService
    {
        void ResizeImage(List<IFormFile> images, string widthString, string heightString);
        byte[] Zip();
    }
}
