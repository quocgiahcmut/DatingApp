using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Repositories.PhotoRepo;

public interface IPhotoRepository
{
    Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos();
    Task<Photo> GetPhotoById(int id);
    void RemovePhoto(Photo photo);
}
