using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Helpers;
using WebApi.Repositories.LikeRepository;
using WebApi.Repositories.MessageRepository;
using WebApi.Repositories.UserRepository;
using WebApi.Services.Photo;
using WebApi.Services.Token;
using WebApi.SignalR;

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
        services.AddSignalR();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<LogUserActivity>();
        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
        services.AddSingleton<PresenceTracker>();

        return services;
    }
}
