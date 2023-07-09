﻿using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Helpers;
using WebApi.Repositories.LikeRepository;
using WebApi.Repositories.MessageRepository;
using WebApi.Repositories.UserRepository;
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
        services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPhotoService, PhotoService>();

        services.AddScoped<LogUserActivity>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

        return services;
    }
}
