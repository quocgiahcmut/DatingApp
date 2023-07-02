using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Helpers;

namespace WebApi.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UserRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MemberDto> GetMemberAsync(string username)
    {
        return await _context.Users
            .Where(u => u.UserName == username)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
    {
        var query = _context.Users
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .AsNoTracking();

        return await PagedList<MemberDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
    }

    public async Task<AppUser> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.Photos)
            .SingleOrDefaultAsync(u => u.UserName == username);
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await _context.Users
            .Include(u => u.Photos)
            .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }
}
