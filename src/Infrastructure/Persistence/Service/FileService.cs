using Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Service
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _baseFolderPath;
        private readonly string _relativeFilePath;

        private string fileId;
        public string FilePath { set => fileId = value; }



        public FileService(IWebHostEnvironment webHostEnvironment, string filePath)
        {
            _webHostEnvironment = webHostEnvironment;
            this._baseFolderPath = $"{_webHostEnvironment.WebRootPath}/{filePath}";

            _relativeFilePath = $"{filePath}";
        }



        public async Task DeleteFile(string _relativeFilePath)
        {
            var fullPath = $"{_webHostEnvironment.WebRootPath}/{_relativeFilePath}";

            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath));
            }

        }

        public async Task<string> UploadFile(IFormFile file)
        {
            #region Try catch version
            /*
             
            try
            {
                if (!System.IO.Directory.Exists(_baseFolderPath)) Directory.CreateDirectory(_baseFolderPath);


                if (file.Length > 0)
                {
                    string extention = file.FileName.Split('.').Last();

                    var fullPath = $"{this._baseFolderPath}/{fileId}.{extention}";
                    var relativePath = $"{this._relativeFilePath}/{fileId}.{extention}";
                    using (var stream = File.Create(fullPath))
                    {
                        await file.CopyToAsync(stream);
                        await stream.FlushAsync();
                        return relativePath;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            } 

             */
            #endregion

            if (!System.IO.Directory.Exists(_baseFolderPath)) Directory.CreateDirectory(_baseFolderPath);


            if (file.Length > 0)
            {
                string extention = file.FileName.Split('.').Last();

                var fullPath = $"{this._baseFolderPath}/{fileId}.{extention}";
                var relativePath = $"{this._relativeFilePath}/{fileId}.{extention}";
                using (var stream = File.Create(fullPath))
                {
                    await file.CopyToAsync(stream);
                    await stream.FlushAsync();
                    return relativePath;
                }
            }
            return null;
        }
    }
}
