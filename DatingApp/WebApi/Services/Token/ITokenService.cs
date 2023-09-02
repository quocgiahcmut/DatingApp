using WebApi.Entities;

namespace WebApi.Services.Token
{
	public interface ITokenService
	{
		Task<string> CreateToken(AppUser user);
	}
}
