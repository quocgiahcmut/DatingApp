using WebApi.Entities;

namespace WebApi.Services.Token
{
	public interface ITokenService
	{
		string CreateToken(AppUser user);
	}
}
