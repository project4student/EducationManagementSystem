using EducationManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EducationManagementSystem.Controllers;

[Authorize(Roles = "Admin,Teacher")]
public class TeacherController : Controller
{
	private readonly EMSContext context;
	private readonly UserManager<Users> userManager;

	public TeacherController(EMSContext context, UserManager<Users> userManager)
	{
		this.context = context;
		this.userManager = userManager;
	}
	public IActionResult Index()
	{
		return View();
	}

	public async Task<IActionResult> AssignHomework()
	{
		var user = await userManager.GetUserAsync(User);
		var result = (
			from s in context.Subject
			join sa in context.SubjectAssigned
			on s.SubjectId equals sa.Id


		)
		return View();
	}
}