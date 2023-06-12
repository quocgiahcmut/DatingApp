using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Entities;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public UsersController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public ActionResult<IEnumerable<AppUser>> GetAllUsers()
		{
			var users = _context.Users.ToList();

			return Ok(users);
		}

		[HttpGet("{id}")]
		public ActionResult<AppUser> GetUser(int id)
		{
			var user = _context.Users.Find(id);

			return Ok(user);
		}
	}
}
