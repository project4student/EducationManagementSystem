using Microsoft.AspNetCore.Mvc;

namespace EducationManagementSystem.Controllers;

public class ErrorController : Controller
{
	[Route("/Error/{statusCode}")]
	public IActionResult StatusCodeHandler(int statusCode)
	{
		switch (statusCode)
		{
			case 404:
				return View("404");
			default:
				return View("404");
		}
	}
}
