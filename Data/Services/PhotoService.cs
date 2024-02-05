using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using MilestoneMotorsWeb.Data.Helpers;
using MilestoneMotorsWeb.Data.Interfaces;

namespace MilestoneMotorsWeb.Data.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary? _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            try
            {
                _cloudinary = new Cloudinary(account);
            }
            catch (Exception)
            {
                _cloudinary = null;
            }
        }

        public async Task<ImageUploadResult?> AddPhotoAsync(IFormFile file)
        {
            if (_cloudinary != null)
            {
                var uploadResult = new ImageUploadResult();
                if (file.Length > 0)
                {
                    using var stream = file.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream)
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
                return uploadResult;
            }
            return null;
        }

        public async Task<DeletionResult?> DeletePhotoAsync(string publicUrl)
        {
            if (_cloudinary != null)
            {
                var publicId = publicUrl.Split('/').Last().Split('.')[0];
                var deleteParams = new DeletionParams(publicId);
                return await _cloudinary.DestroyAsync(deleteParams);
            }
            return null;
        }
    }
}
