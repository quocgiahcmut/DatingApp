using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Repositories.LIke;

public interface ILikeRepository
{
    Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);
    Task<AppUser> GetUserWithLikes(int userId);
    Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId);
}
