using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Services.Token;

namespace WebApi.Controllers;

public class AccountController : BaseApiController
{
	private readonly ApplicationDbContext _context;
	private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountController(ApplicationDbContext context, ITokenService tokenService, IMapper mapper)
	{
		_context = context;
		_tokenService = tokenService;
        _mapper = mapper;
    }

	[HttpPost("register")] // /api/account/register?username=quocgia&password=password
	public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
	{
		if (await IsUserExistsAsync(registerDto.Username)) return BadRequest("Username is taken");

		var user = _mapper.Map<AppUser>(registerDto);

		using var hmac = new HMACSHA512();


		user.UserName = registerDto.Username.ToLower();
		user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
		user.PasswordSalt = hmac.Key;

		_context.Users.Add(user);
		await _context.SaveChangesAsync();

		return Ok(new UserDto
		{
			Username = user.UserName,
			Token = _tokenService.CreateToken(user),
			PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url,
			KnownAs = user.KnownAs,
			Gender = user.Gender
		});
	}

	[HttpPost("login")]
	public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
	{
		var user = await _context.Users
			.Include(p => p.Photos)
			.SingleOrDefaultAsync(u => u.UserName == loginDto.Username);

		if (user is null) return Unauthorized("Invalid username");

		using var hmac = new HMACSHA512(user.PasswordSalt);

		var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

		for (int i = 0; i < computedHash.Length; i++)
		{
			if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
		}

		return Ok(new UserDto
		{
			Username = user.UserName,
			Token = _tokenService.CreateToken(user),
			PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url,
			KnownAs = user.KnownAs,
			Gender = user.Gender
		});
	}

	private async Task<bool> IsUserExistsAsync(string username)
	{
		return await _context.Users.AnyAsync(au => au.UserName == username.ToLower());
	}
}
