using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationManagementSystem.Controllers;

[Authorize(Roles = "Admin,Teacher")]
public class TeacherController : Controller
{
	public IActionResult Index()
	{
		return View();
	}

	public IActionResult AssignHomework()
	{
		return View();
	}
}