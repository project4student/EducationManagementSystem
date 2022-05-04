using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EducationManagementSystem.Models;
using EducationManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace EducationManagementSystem.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
	private readonly UserManager<Users> userManager;
	private readonly RoleManager<IdentityRole> roleManager;
	private readonly EMSContext context;
	private readonly SignInManager<Users> signinManager;
	private readonly IWebHostEnvironment webHostEnvironment;

	public AdminController(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, EMSContext context, SignInManager<Users> signinManager, IWebHostEnvironment webHostEnvironment)
	{
		this.userManager = userManager;
		this.roleManager = roleManager;
		this.context = context;
		this.signinManager = signinManager;
		this.webHostEnvironment = webHostEnvironment;

	}
	[HttpGet]
	public IActionResult AssignSubject()
	{
		var Teacher = context.User.Where(t => t.UserTypeId == 2).ToList();
		var assignSubject = new AssignSubjectViewModel()
		{
			Teacher = Teacher
		};
		return View(assignSubject);
	}
	[HttpPost]
	public IActionResult AssignSubject(AssignSubjectViewModel assignSubject)
	{
		try
		{
			if (ModelState.IsValid)
			{
				var subjectAssign = new SubjectAssigned()
				{
					SubjectId = assignSubject.SubjectId,
					TeacherId = assignSubject.TeacherId
				};
				context.SubjectAssigned.Add(subjectAssign);
				context.SaveChanges();
				return Json(new { success = "Subject Assigned Successfully" });
			}
			return Json(new { err = "Select Valid field" });
		}
		catch (Exception e)
		{
			Console.Write(e.Message);
			return Json(new { err = "Internal Server Error" });
		}
	}
	public IActionResult AddSchedules()
	{
		return View();
	}
	[HttpPost]
	public IActionResult AddSchedules(IFormCollection data, IFormFile scheduleFile)
	{
		try
		{
#nullable disable
			var ClassId = Int32.Parse(data["Class"][0]);
			var Type = Int32.Parse(data["Type"][0]);
			var isExam = Type == 2 ? true : false;
			var s = context.Schedules.Where(s => s.ClassId == ClassId && s.isExam == isExam).Count();
			if (s > 0)
			{
				return Json(new { err = "Schedule already exist, to upload new one delete old schedule" });
			}
			var schedules = new Schedule();
			if (ClassId != 0 && Type != 0)
			{
				schedules.ClassId = ClassId;
				schedules.isExam = isExam;
				if (scheduleFile != null)
				{
					var uploadedFile = "Uploads/LectureScheduls/";
					uploadedFile += Guid.NewGuid().ToString() + "_" + scheduleFile.FileName;
					var serverFolder = Path.Combine(webHostEnvironment.WebRootPath, uploadedFile);
					using (var filestream = new FileStream(serverFolder, FileMode.Create))
					{
						scheduleFile.CopyTo(filestream);
					}
					schedules.FilePath = uploadedFile;
					schedules.UploadedDate = DateTime.Now;
					schedules.Description = data["description"];
					context.Schedules.Add(schedules);
					context.SaveChanges();
					return Json(new { success = "Schedule Uploaded Successfuly" });
				}
			}
			return Json(new { err = "Enter valid field" });
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return Json(new { err = "Internal server Error" });
		}
	}
	public IActionResult GetScheduleList()
	{
		var schedule = context.Schedules.ToList();
		return View(schedule);
	}
	public IActionResult DeleteSchedules(int id)
	{
		try
		{
			var schedule = context.Schedules.FirstOrDefault(s => s.Id == id);
			if (schedule == null)
			{
				return Json(new { err = "Schedule  not found" });
			}
			context.Remove(schedule);
			context.SaveChanges();
			return Json(new { success = "Schedule deleted successfuly" });
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return Json(new { err = "Internal server error" });
		}
	}
	public IActionResult GetSubjectList(int classId)
	{
		var Subject = context.Subject.Where(s => s.ClassId == classId).ToList();
		return Json(new { subject = Subject });
	}

	[AllowAnonymous]
	public IActionResult GetSchedule(int Id)
	{
		try
		{
			if (User.Identity.IsAuthenticated && (User.IsInRole("Admin") || User.IsInRole("Student") || User.IsInRole("Teacher")))
			{
				var schedule = context.Schedules.Where(s => s.Id == Id).Select(s => s.FilePath).FirstOrDefault();
				string path = $"wwwroot/{schedule}";
				byte[] pdf = System.IO.File.ReadAllBytes(path);
				MemoryStream ms = new MemoryStream(pdf);
				return new FileStreamResult(ms, "application/pdf");
			}
			else
			{
				return RedirectToAction("Login", "Account");
			}
		}
		catch
		{
			return Json(new { isSuccess = false, err = "Internal Server Error" });
		}
	}

	public IActionResult Notice()
	{
		return View();
	}
	[HttpPost]
	public IActionResult Notice(IFormCollection data)
	{
		try
		{
			if (data.Keys.Any())
			{
				var notice = new Notice()
				{
					ClassId = Int32.Parse(data["Class"]),
					details = data["detail"],
					CreatedDate = DateTime.Now
				};
				context.Add(notice);
				context.SaveChanges();
				return Json(new { success = "Notice add succefully" });
			}
			else
			{
				return Json(new { err = "Enter valid fields" });
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return Json(new { err = "Internal Server Error" });
		}
	}
	public IActionResult UserManagement()
	{
		var users = context.User.ToList();
		return View(users);
	}
}