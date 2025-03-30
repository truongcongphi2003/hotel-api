using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace thda.Services
{
    public class ImageService : IImageService
    {
        private readonly IConfiguration _config;
        private readonly Cloudinary _cloudinary;
        public ImageService(IConfiguration config, Cloudinary cloudinary)
        {
            _config = config;
            _cloudinary = cloudinary;
        }
        public async Task<UploadResult> UploadAsync(IFormFile image)
        {
            var pictureSize = Convert.ToInt64(_config["PhotoSettings:Size"]);
            if (image.Length > pictureSize)
            {
                throw new ArgumentException("Kích thước tập tin vượt quá");
            }
            var pictureFormat = false;

            var listOfImageExtensions = new List<string>() { ".jpg", ".png", ".jpeg" };

            foreach (var item in listOfImageExtensions)
            {
                if ((image.FileName.ToLower().EndsWith(item)))
                {
                    pictureFormat = true;
                    break;
                }
            }

            if (pictureFormat == false)
            {
                throw new ArgumentException("Định dạng tập tin không được hỗ trợ");
            }

            var uploadResult = new ImageUploadResult();

            using (var imageStream = image.OpenReadStream())
            {
                string filename = Guid.NewGuid().ToString() + image.FileName;

                uploadResult = await _cloudinary.UploadAsync(new ImageUploadParams()
                {
                    File = new FileDescription(filename + Guid.NewGuid().ToString(), imageStream),
                    PublicId = "Hotels/" + filename,
                    Transformation = new Transformation().Crop("thumb").Gravity("face").Width(150)
                });
            }
            var s = uploadResult.PublicId;

            return uploadResult;
        }

        public async Task<DelResResult> DeleteResourcesAsync(string url)
        {
            Uri uri = new Uri(url);
            string filename = System.IO.Path.GetFileNameWithoutExtension(uri.LocalPath);
            DelResParams delParams = new DelResParams
            {
                PublicIds = new List<string> { "Hotels/" + filename },
                //All = true,
                KeepOriginal = false,
                Invalidate = true
            };

            DelResResult deletionResult = await _cloudinary.DeleteResourcesAsync(delParams);
            if (deletionResult.Error != null)
            {
                throw new ApplicationException($"" +
                    $"an error occured in method: " +
                    $"{nameof(DeleteResourcesAsync)}" +
                    $" class: {nameof(ImageService)}");
            }

            return deletionResult;
        }

        public async Task<UploadResult> UploadImageAsync(IFormFile image)
        {
            var pictureSize = Convert.ToInt64(_config["PhotoSettings:Size"]);
            if (image.Length > pictureSize)
            {
                throw new ArgumentException("Kích thước tập tin vượt quá");
            }
            var pictureFormat = false;

            var listOfImageExtensions = new List<string>() { ".jpg", ".png", ".jpeg" };

            foreach (var item in listOfImageExtensions)
            {
                if ((image.FileName.ToLower().EndsWith(item)))
                {
                    pictureFormat = true;
                    break;
                }
            }

            if (pictureFormat == false)
            {
                throw new ArgumentException("Định dạng tập tin không được hỗ trợ");
            }

            var uploadResult = new ImageUploadResult();

            using (var imageStream = image.OpenReadStream())
            {
                string filename = Guid.NewGuid().ToString() + image.FileName;

                uploadResult = await _cloudinary.UploadAsync(new ImageUploadParams()
                {
                    File = new FileDescription(filename + Guid.NewGuid().ToString(), imageStream),
                    PublicId = "Hotels/" + filename                    
                });
            }
            var s = uploadResult.PublicId;

            return uploadResult;
        }

        public async Task<List<UploadResult>> UploadImagesAsync(IEnumerable<IFormFile> images)
        {
            var uploadResults = new List<UploadResult>();

            foreach (var image in images)
            {
                // Gọi phương thức UploadImageForRoomAsync cho từng ảnh
                var result = await UploadImageAsync(image);
                uploadResults.Add(result);
            }

            return uploadResults;
        }

       
    }
}
