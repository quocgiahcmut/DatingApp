using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Repositories.LIke;

public interface ILikeRepository
{
    Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);
    Task<AppUser> GetUserWithLikes(int userId);
    Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
}
