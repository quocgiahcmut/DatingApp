using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Helpers;
using WebApi.Repositories.User;
using WebApi.Services.Photo;

namespace WebApi.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;
	private readonly IPhotoService _photoService;

	public UsersController(IUserRepository userRepository,
						IMapper mapper,
						IPhotoService photoService)
	{
		_userRepository = userRepository;
		_mapper = mapper;
		_photoService = photoService;
	}

	[HttpGet]
	public async Task<ActionResult<PagedList<MemberDto>>> GetAllUsers([FromQuery]UserParams userParams)
	{
		var users = await _userRepository.GetMembersAsync(userParams);

		Response.AddPaginationHeader(new PaginationHeader(users.CurrnetPage,
                                                    users.PageSize,
                                                    users.TotalCount,
                                                    users.TotalPages));

		return Ok(users);
	}

	[HttpGet("{username}")]
	public async Task<ActionResult<MemberDto>> GetUser(string username)
	{
		var userToReturn = await _userRepository.GetMemberAsync(username);

		return userToReturn;
	}

	[HttpPut]
	public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
	{
		var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

		if (user == null) return NotFound();

		_mapper.Map(memberUpdateDto, user);

		if (await _userRepository.SaveAllAsync()) return NoContent();

		return BadRequest("Failed to update user");
	}

	[HttpPost("add-photo")]
	public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
	{
		var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

		if (user == null) return NotFound();

		var result = await _photoService.AddPhotoAsync(file);

		if (result.Error != null) return BadRequest(result.Error.Message);

		var photo = new Photo
		{
			Url = result.SecureUrl.AbsoluteUri,
			PublicId = result.PublicId,
		};

		if (user.Photos.Count == 0) photo.IsMain = true;

		user.Photos.Add(photo);

		if (await _userRepository.SaveAllAsync())
		{
			return CreatedAtAction(nameof(GetUser), new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));
		}

		return BadRequest("Problem adding photo");
	}

	[HttpPut("set-main-photo/{photoId}")]
	public async Task<ActionResult> SetMainPhoto(int photoId)
	{
		var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

		if (user == null) return NotFound();

		var photo = user.Photos.Where(p => p.Id == photoId).FirstOrDefault();

		if (photo == null) return NotFound();

		if (photo.IsMain) return BadRequest("This is already your main photo");

		var currentMain = user.Photos.Where(p => p.IsMain).FirstOrDefault();
		if (currentMain != null) currentMain.IsMain = false;
		photo.IsMain = true;

		if (await _userRepository.SaveAllAsync()) return NoContent();

		return BadRequest("Problem setting the main photo");
	}

	[HttpDelete("delete-photo/{photoId}")]
	public async Task<ActionResult> DeletePhoto(int photoId)
	{
		var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

		var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

		if (photo == null) return NotFound();

		if (photo.IsMain) return BadRequest("You cannot delete your main photo");

		if (photo.PublicId != null)
		{
			var result = await _photoService.DeletePhotoAsync(photo.PublicId);
			if (result.Error != null) return BadRequest(result.Error.Message);
		}

		user.Photos.Remove(photo);

		if (await _userRepository.SaveAllAsync()) return Ok();

		return BadRequest("Problem deleting photo");
	}
}
