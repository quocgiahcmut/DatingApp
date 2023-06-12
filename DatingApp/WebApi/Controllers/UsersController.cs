using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Entities;

namespace WebApi.Controllers;

public class UsersController : BaseApiController
{
	private readonly ApplicationDbContext _context;

	public UsersController(ApplicationDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<AppUser>>> GetAllUsers()
	{
		var users = await _context.Users.ToListAsync();

		return Ok(users);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<AppUser>> GetUser(int id)
	{
		var user = await _context.Users.FindAsync(id);

		return Ok(user);
	}
}
