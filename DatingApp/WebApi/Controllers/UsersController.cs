﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Repositories.User;

namespace WebApi.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;

	public UsersController(IUserRepository userRepository, IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllUsers()
	{
		var usersToReturn = await _userRepository.GetMembersAsync();

		return Ok(usersToReturn);
	}

	[HttpGet("{username}")]
	public async Task<ActionResult<MemberDto>> GetUser(string username)
	{
		var userToReturn = await _userRepository.GetMemberAsync(username);

		return userToReturn;
	}
}
