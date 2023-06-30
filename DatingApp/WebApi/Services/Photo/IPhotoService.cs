using CloudinaryDotNet.Actions;

namespace WebApi.Services.Photo;

public interface IPhotoService
{
	Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
	Task<DeletionResult> DeletePhotoAsync(string publicId);
}
