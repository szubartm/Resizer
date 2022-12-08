using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ResizerWEB.ImageUpload;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace ResizerWEB.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Directory.CreateDirectory("wwwroot\\converted");
            string[] filepaths = Directory.GetFiles("wwwroot\\converted\\");
            foreach(var file in filepaths)
            {
                System.IO.File.Delete(file);
            }
        }

        public FileResult OnPost(List<IFormFile> images, string widthString, string heightString)
        {
            var resize = new ResizeImageService();
            resize.ResizeImage(images, widthString, heightString);
            
            var finalResult = resize.Zip();
            
            return File(finalResult, "application/zip", "ConvertedImages.zip");
           
        }
    }
}