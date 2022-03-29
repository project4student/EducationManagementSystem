using Microsoft.AspNetCore.Mvc;

namespace EducationManagementSystem.Controllers;

public class AccountController : Controller
{
	public IActionResult Login()
	{
		return View();
	}
}