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

	public AccountController(ApplicationDbContext context, ITokenService tokenService)
	{
		_context = context;
		_tokenService = tokenService;
	}

	[HttpPost("register")] // /api/account/register?username=quocgia&password=password
	public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
	{
		if (await IsUserExistsAsync(registerDto.Username)) return BadRequest("Username is taken");

		using var hmac = new HMACSHA512();

		var user = new AppUser
		{
			UserName = registerDto.Username.ToLower(),
			PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
			PasswordSalt = hmac.Key
		};

		_context.Users.Add(user);
		await _context.SaveChangesAsync();

		return Ok(new UserDto
		{
			Username = user.UserName,
			Token = _tokenService.CreateToken(user)
			//PhotoUrl = user.Photos.Where(p => p.IsMain).FirstOrDefault().Url
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
			PhotoUrl = user.Photos.Where(p => p.IsMain).FirstOrDefault().Url
		});
	}

	private async Task<bool> IsUserExistsAsync(string username)
	{
		return await _context.Users.AnyAsync(au => au.UserName == username.ToLower());
	}
}
