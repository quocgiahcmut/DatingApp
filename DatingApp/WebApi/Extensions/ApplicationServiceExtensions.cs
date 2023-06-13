using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Services.Token;

namespace WebApi.Extensions;

public static class ApplicationServiceExtensions
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
	{
		services.AddDbContext<ApplicationDbContext>(opt =>
		{
			opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
		});
		services.AddCors();
		services.AddScoped<ITokenService, TokenService>();

		return services;
	}
}
