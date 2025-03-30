using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace thda.Services
{
    public interface IImageService
    {
        Task<UploadResult> UploadAsync(IFormFile image);
        Task<DelResResult> DeleteResourcesAsync(string publicId);

        Task<UploadResult> UploadImageAsync(IFormFile image);
        Task<List<UploadResult>> UploadImagesAsync(IEnumerable<IFormFile> images);
        
    }
}
