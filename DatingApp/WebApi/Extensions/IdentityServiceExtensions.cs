using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApi.Extensions;

public static class IdentityServiceExtensions
{
	public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
	{
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(opt =>
			{
				opt.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("TokenKey"))),
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});

		return services;
	}
}
