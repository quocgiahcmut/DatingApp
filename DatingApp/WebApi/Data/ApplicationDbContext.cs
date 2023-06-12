using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<AppUser> Users { get; set; }
	}
}
