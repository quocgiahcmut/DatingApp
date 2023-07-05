﻿using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Repositories.LIke;
using WebApi.Repositories.User;

namespace WebApi.Controllers;

public class LikesController : BaseApiController
{
    private readonly ILikeRepository _likeRepository;
    private readonly IUserRepository _userRepository;

    public LikesController(ILikeRepository likeRepository, IUserRepository userRepository)
    {
        _likeRepository = likeRepository;
        _userRepository = userRepository;
    }

    [HttpPost("{username}")]
    public async Task<ActionResult> AddLike(string username)
    {
        var sourceUserId = User.GetUserId();
        var likedUser = await _userRepository.GetUserByUsernameAsync(username);
        var sourceUser = await _likeRepository.GetUserWithLikes(sourceUserId);

        if (likedUser == null) return NotFound();

        if (sourceUser.UserName == username) return BadRequest("You cannot like yourself");

        var userLike = await _likeRepository.GetUserLike(sourceUserId, likedUser.Id);

        if (userLike != null) return BadRequest("You already like this user");

        userLike = new UserLike
        {
            SourceUserId = sourceUserId,
            TargetUserId = likedUser.Id
        };

        sourceUser.LikedUsers.Add(userLike);

        if (await _userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Failed to like user");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes(string predicate)
    {
        var users = await _likeRepository.GetUserLikes(predicate, User.GetUserId());

        return Ok(users);
    }
}
