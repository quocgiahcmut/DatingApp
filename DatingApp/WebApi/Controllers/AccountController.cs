using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
	private readonly UserManager<AppUser> _userManager;
	private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
	{
		_userManager = userManager;
		_tokenService = tokenService;
        _mapper = mapper;
    }

	[HttpPost("register")] // /api/account/register?username=quocgia&password=password
	public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
	{
		if (await IsUserExistsAsync(registerDto.Username)) return BadRequest("Username is taken");

		var user = _mapper.Map<AppUser>(registerDto);

		user.UserName = registerDto.Username.ToLower();

		var userResult = await _userManager.CreateAsync(user, registerDto.Password);

		if (!userResult.Succeeded) return BadRequest(userResult.Errors);

		var roleResult = await _userManager.AddToRoleAsync(user, "Member");

		if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

		return Ok(new UserDto
		{
			Username = user.UserName,
			Token = await _tokenService.CreateToken(user),
			KnownAs = user.KnownAs,
			Gender = user.Gender
		});
	}

	[HttpPost("login")]
	public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
	{
		var user = await _userManager.Users
			.Include(p => p.Photos)
			.SingleOrDefaultAsync(u => u.UserName == loginDto.Username);

		if (user is null) return Unauthorized("Invalid username");

		var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

		if (!result) return Unauthorized("Invalid Password");

		return Ok(new UserDto
		{
			Username = user.UserName,
			Token = await _tokenService.CreateToken(user),
			PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url,
			KnownAs = user.KnownAs,
			Gender = user.Gender
		});
	}

	private async Task<bool> IsUserExistsAsync(string username)
	{
		return await _userManager.Users.AnyAsync(au => au.UserName == username.ToLower());
	}
}
