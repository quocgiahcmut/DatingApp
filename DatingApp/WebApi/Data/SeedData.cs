using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using WebApi.Entities;

namespace WebApi.Data;

public class SeedData
{
	public static async Task SeedUsers(UserManager<AppUser> userManager)
	{
		if (await userManager.Users.AnyAsync()) { return; }

		var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

		var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
		var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

		foreach (var user in users)
		{
			user.UserName = user.UserName.ToLower();

			await userManager.CreateAsync(user, "p@Ssw0rd");
		}
	}
}
