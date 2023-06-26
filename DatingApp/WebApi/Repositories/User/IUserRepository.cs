using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Repositories.User;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser> GetUserByIdAsync(int id);
    Task<AppUser> GetUserByUsernameAsync(string username);
    Task<IEnumerable<MemberDto>> GetMembersAsync(); 
    Task<MemberDto> GetMemberAsync(string username);
}
