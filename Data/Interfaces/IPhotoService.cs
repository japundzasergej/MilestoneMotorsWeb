using CloudinaryDotNet.Actions;

namespace MilestoneMotorsWeb.Data.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult?> AddPhotoAsync(IFormFile file);
        Task<DeletionResult?> DeletePhotoAsync(string publicUrl);
    }
}
