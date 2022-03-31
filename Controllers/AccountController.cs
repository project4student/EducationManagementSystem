using System.ComponentModel.DataAnnotations;
using EducationManagementSystem.Models;
using EducationManagementSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EducationManagementSystem.Controllers;

public class AccountController : Controller
{
	private readonly EMSContext context;
	private readonly UserManager<Users> userManager;
	private readonly SignInManager<Users> signInManager;
	private readonly RoleManager<IdentityRole> roleManager;

	public AccountController(EMSContext context, UserManager<Users> userManager, SignInManager<Users> signInManager, RoleManager<IdentityRole> roleManager)
	{
		this.context = context;
		this.userManager = userManager;
		this.signInManager = signInManager;
		this.roleManager = roleManager;
	}
	[HttpGet]
	public IActionResult Login()
	{
		return View();
	}

	[HttpGet]
	public async Task<IActionResult> CreateRoles()
	{
		try
		{
			IdentityRole role = new IdentityRole("Admin");
			IdentityRole role2 = new IdentityRole("Teacher");
			IdentityRole role3 = new IdentityRole("Student");
			var result = await roleManager.CreateAsync(role);
			var result2 = await roleManager.CreateAsync(role2);
			var result3 = await roleManager.CreateAsync(role3);
			if (result.Succeeded && result2.Succeeded && result3.Succeeded) return Json(new { success = "Created Roles !" });
			else
			{
				string err = "";
				foreach (var e in result.Errors) err += e.Description + ",";
				foreach (var e in result2.Errors) err += e.Description + ",";
				foreach (var e in result3.Errors) err += e.Description + ",";
				return Json(new { err = err });
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return Json(new { err = "Internal Server Error !" });
		}
	}

	[HttpPost]
	public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminViewModel model)
	{
		try
		{
			if (ModelState.IsValid)
			{
				var d = model.DOB.Split("/");
				var newAdmin = new Users()
				{
					Email = model.Email,
					FirstName = model.FirstName,
					LastName = model.LastName,
					MiddleName = model.MiddleName,
					UserTypeId = 3,
					PhoneNumber = model.PhoneNumber,
					UserName = model.Email,
					DateOfBirth = new DateTime(Convert.ToInt32(d[2]), Convert.ToInt32(d[0]), Convert.ToInt32(d[1])),
					Address = model.Address
				};
				var result = await userManager.CreateAsync(newAdmin, model.Password);
				if (result.Succeeded)
				{
					var user = await userManager.FindByEmailAsync(model.Email);
					await userManager.AddToRoleAsync(user, "Admin");
					return Json(new { success = "Created Admin !" });
				}
				else
				{
					string str = "";
					foreach (var e in result.Errors) str += e.Description + ",";
					return Json(new { err = str });
				}
			}
			else return Json(new { err = "Enter Fields Properly !" });
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return Json(new { err = "Internal Server Error !" });
		}
	}

	[HttpPost]
	public IActionResult Login([FromBody] LoginViewModel model)
	{
		return RedirectToAction("Index", "Home");
	}
}
