using EducationManagementSystem.Models;
using EducationManagementSystem.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EducationManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;

namespace EducationManagementSystem.Controllers;

public class AccountController : Controller
{
	private readonly UserManager<Users> userManager;
	private readonly RoleManager<IdentityRole> roleManager;
	private readonly EMSContext context;
	private readonly SignInManager<Users> signinManager;
	private readonly IWebHostEnvironment webHostEnvironment;
	private readonly IDataProtector protector;
	private readonly Email SendEmail;
	public readonly string Key = "Chintan@Mihir@Deep";


	public AccountController(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, EMSContext context, SignInManager<Users> signinManager, IWebHostEnvironment webHostEnvironment, IDataProtectionProvider protector, Email email)
	{
		this.userManager = userManager;
		this.roleManager = roleManager;
		this.context = context;
		this.signinManager = signinManager;
		this.webHostEnvironment = webHostEnvironment;
		this.SendEmail = email;
		this.protector = protector.CreateProtector(Key);
	}
	public IActionResult Login()
	{
		return View();
	}
	[HttpPost]
	public async Task<IActionResult> Login(LoginViewModel model)
	{
		try
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByEmailAsync(model.Email);
				if (user != null)
				{
					var result = await signinManager.PasswordSignInAsync(user, model.Password, true, true);
					if (result.Succeeded)
					{
						if (user.UserTypeId == 1) return Json(new { success = "Login successful !", redirectUrl = Url.Action("Index", "Student") });
						if (user.UserTypeId == 2) return Json(new { success = "Login successful !", redirectUrl = Url.Action("Index", "Teacher") });
						if (user.UserTypeId == 3) return Json(new { success = "Login successful !", redirectUrl = Url.Action("CreateUser", "Account") });
					}
					else
					{
						if (result.IsNotAllowed) return Json(new { err = "Authentication Failed !" });
						if (result.IsLockedOut) return Json(new { err = "You have entered wrong Password too many times wait for sometime and try again later or reset your Password !" });
					}
				}
			}
			return Json(new { err = "Authentication Failed !" });
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return Json(new { err = "Internal Server Error !" });
		}
	}
	[HttpPost]
	public IActionResult GetCredentials(string Email)
	{
		try
		{
			if (Email != null)
			{
				var EncPass = (from user in context.User
							   join encpass in context.EncryptedPasswords
							   on user.Id equals encpass.UserId
							   where user.Email == Email
							   select (encpass.EncPassword)).FirstOrDefault();

				if (EncPass != null)
				{
					var password = protector.Unprotect(EncPass);
					var EmailViewModel = new EmailViewModel()
					{
						To = new List<string> { Email },
						isHTML = true,
						Body = "You password is " + password,
						Subject = "Regarding Credentials"
					};
					var result = SendEmail.sendMail(EmailViewModel);
					if (result)
						return Json(new { success = "Credentials successfuly sent to your Email" });

					return Json(new { err = "Somthig went wrong Email not sent" });
				}
				return Json(new { success = "Sorry!! Credentials not found" });
			}
			return Json(new { err = "Enter Email" });
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return Json(new { err = "Internal Server Error !" });
		}
	}
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
	[Authorize(Roles = "Admin")]
	public IActionResult CreateUser()
	{
		return View();
	}
	[Authorize(Roles = "Admin")]
	[HttpPost]
	public async Task<IActionResult> CreateUser(UserViewModels createTeacherViewModel)
	{
		try
		{
			if (ModelState.IsValid)
			{
				var user = new Users()
				{
					FirstName = createTeacherViewModel.FirstName,
					LastName = createTeacherViewModel.LastName,
					MiddleName = createTeacherViewModel.MiddleName,
					Email = createTeacherViewModel.Email,
					Address = createTeacherViewModel.Address,
					DateOfBirth = createTeacherViewModel.DateOfBirth,
					PhoneNumber = createTeacherViewModel.PhoneNumber,
					UserTypeId = createTeacherViewModel.UserTypeId,
					UserName = createTeacherViewModel.Email
				};
				if (user.UserTypeId == 1)
				{
					user.ClassId = createTeacherViewModel.ClassId;
					user.AdmissionDate = DateTime.Now;
					var maxGnrNo = context.User.Max(u => u.GNRNo);
					user.GNRNo = maxGnrNo != null ? maxGnrNo + 1 : 1;
					var rollNumber = context.User.Where(u => u.ClassId == createTeacherViewModel.ClassId && u.RollNo == createTeacherViewModel.RollNumber).FirstOrDefault();
					if (rollNumber != null)
					{
						return Json(new { err = "Roll Number already exists for this Class" });
					}
					user.RollNo = createTeacherViewModel.RollNumber;
				}


				var result = await userManager.CreateAsync(user, createTeacherViewModel.Password);
				if (result.Succeeded)
				{
					var user1 = await userManager.FindByEmailAsync(createTeacherViewModel.Email);
					var role = createTeacherViewModel.UserTypeId == 1 ? "Student" : "Teacher";
					await userManager.AddToRoleAsync(user1, role);
					if (createTeacherViewModel.ProfilePicture != null)
					{
						var uploadedFile = "ProfilePicture/";
						uploadedFile += Guid.NewGuid().ToString() + createTeacherViewModel.ProfilePicture.FileName;
						var serverFolder = Path.Combine(webHostEnvironment.WebRootPath, uploadedFile);
						using (var filestream = new FileStream(serverFolder, FileMode.Create))
						{
							createTeacherViewModel.ProfilePicture.CopyTo(filestream);
						}
						user.Profile = uploadedFile;

					}
					var EncPass = protector.Protect(createTeacherViewModel.Password);
					context.EncryptedPasswords.Add(new EncryptedPassword()
					{
						EncPassword = EncPass,
						UserId = user1.Id
					});
					context.SaveChanges();
					return Json(new { success = "User Added Successfuly" });
				}
				string str = "";
				foreach (var e in result.Errors)
				{
					str += e.Description + ", ";
				}
				return Json(new { err = str });
			}
			return Json(new { err = "Enter Proper Fields !" });
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return Json(new { err = "Internal Server Error !" });
		}
	}

	public IActionResult AccessDenied()
	{
		return View();
	}

	[Authorize]
	public async Task<IActionResult> Logout()
	{
		await signinManager.SignOutAsync();
		return RedirectToAction("Index", "Home");
	}
}