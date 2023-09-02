﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using WebApi.Entities;

namespace WebApi.Data;

public class SeedData
{
	public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
	{
		if (await userManager.Users.AnyAsync()) { return; }

		var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

		var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
		var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

		var roles = new List<AppRole>()
		{
			new AppRole { Name = "Member" },
			new AppRole { Name = "Admin" },
			new AppRole { Name = "Moderator" }
		};


        foreach (var role in roles)
        {
            var result = await roleManager.CreateAsync(role);
			Console.WriteLine(result.Succeeded);
        }

        foreach (var user in users)
		{
			user.UserName = user.UserName.ToLower();

			await userManager.CreateAsync(user, "Abc123!!!");
			await userManager.AddToRoleAsync(user, "Member");
		}

		var admin = new AppUser
		{
			UserName = "admin",
		};

		await userManager.CreateAsync(admin, "Abc123!!!");
		await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
	}
}
