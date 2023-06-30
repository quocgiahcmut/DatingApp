using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Helpers;
using WebApi.Repositories.User;
using WebApi.Services.Photo;
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

		services.AddScoped<IUserRepository, UserRepository>();

		services.AddScoped<ITokenService, TokenService>();
		services.AddScoped<IPhotoService, PhotoService>();

		services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

		services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

		return services;
	}
}
