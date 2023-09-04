using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Helpers;

namespace WebApi.Repositories.UserRepository;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser> GetUserByIdAsync(int id);
    Task<AppUser> GetUserByUsernameAsync(string username);
    Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
    Task<string> GetUserGender(string username);
    Task<MemberDto> GetMemberAsync(string username, bool isCurrentUser);
    Task<AppUser> GetUserByPhotoId(int photoId);
}
