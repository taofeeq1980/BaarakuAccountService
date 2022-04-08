using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace ApplicationServices.Shared.Utilities
{
    public static class Util
    {
        public static string RandomDigits(int length = 9)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = string.Concat(s, random.Next(10).ToString());
            return s;
        }

        public static string SaveImageAndReturnFilepath(string ImgStr, string MiMeType, IHostingEnvironment _env)
        {
            var absolutepath = "Documents_" + DateTime.UtcNow.ToString("ddMMMyyyy");
            var path = Path.Combine(_env.ContentRootPath, absolutepath); //Path
                                                                         //Check if directory exist
            if (!Directory.Exists(path))
            {
                _ = Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }
            string imageName = Guid.NewGuid() + "." + MiMeType.Replace(".", "");
            //set the image path
            string imgPath = Path.Combine(path, imageName);
            byte[] imageBytes = Convert.FromBase64String(ImgStr);
            File.WriteAllBytes(imgPath, imageBytes);
            return absolutepath + "/" + imageName;
        }
    }
}
