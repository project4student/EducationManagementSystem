using EducationManagementSystem.Models;
using EducationManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EducationManagementSystem.Controllers;

[Authorize(Roles = "Admin,Teacher")]
public class TeacherController : Controller
{
	private readonly EMSContext context;
	private readonly UserManager<Users> userManager;
	private IWebHostEnvironment webHostEnvironment;

	public TeacherController(EMSContext context, UserManager<Users> userManager, IWebHostEnvironment webHostEnvironment)
	{
		this.context = context;
		this.userManager = userManager;
		this.webHostEnvironment = webHostEnvironment;
	}
	public async Task<IActionResult> Index()
	{
		var user = await userManager.GetUserAsync(User);
		var dashboard = (from u in context.User
						 join asub in context.SubjectAssigned on u.Id equals asub.TeacherId
						 join sub in context.Subject on asub.SubjectId equals sub.SubjectId
						 where u.Id == user.Id
						 select new TeacherDashboardViewModel
						 {
							 ClassId = sub.ClassId,
							 SubjectName = sub.SubjectName
						 }
						).OrderBy(c => c.ClassId).ToList();
		return View(dashboard);
	}

	public IActionResult StudentList(int Id)
	{
		var student = context.User.Where(u => u.UserTypeId == 1 && u.ClassId == Id).ToList();
		var schedule = context.Schedules.Where(s => s.ClassId == Id).ToList();

		var model = new ScheduleAndStudentViewModel()
		{
			student = student,
			schedule = schedule
		};
		return View(model);
	}

	public async Task<IActionResult> AssignHomework()
	{
		var user = await userManager.GetUserAsync(User);
		var subjectList = (
			from u in context.User
			from sub in context.Subject
			join assignSub in context.SubjectAssigned on new { TeacherId = u.Id, SubjectId = sub.SubjectId } equals new { TeacherId = assignSub.TeacherId, SubjectId = assignSub.SubjectId }
			where u.Id == user.Id
			select new SubjectList
			{
				SubjectName = sub.SubjectName,
				SubjectId = sub.SubjectId,
				ClassId = sub.ClassId
			}).OrderBy(s => s.ClassId).ToList();

		return View(new AssignHomeWorkSubmitViewModel()
		{
			SubjectLists = subjectList
		});
	}
	[HttpPost]
	public IActionResult AssignHomework(AssignHomeWorkSubmitViewModel model)
	{
		try
		{

			if (ModelState.IsValid)
			{
				var Homework = new Homework()
				{
					ClassId = model.ClassId,
					SubectId = model.SubjectId,
					Title = model.Title,
					CreatedDate = DateTime.Parse(model.StartDate),
					DueDate = DateTime.Parse(model.DueDate),
					Description = model.Description,
				};
				if (model.HomeworkFile != null)
				{
					var uploadedFile = "Uploads/HomeWork/";
					uploadedFile += Guid.NewGuid().ToString() + "_" + model.HomeworkFile.FileName;
					var serverFolder = Path.Combine(webHostEnvironment.WebRootPath, uploadedFile);
					using (var filestream = new FileStream(serverFolder, FileMode.Create))
					{
						model.HomeworkFile.CopyTo(filestream);
					}
					Homework.AssignedHomeworkFilePath = uploadedFile;
					context.Homeworks.Add(Homework);
					context.SaveChanges();
					return Json(new { success = "Homework assigned Successfuly" });
				}
				else
				{
					return Json(new { err = "Select Homework File" });
				}
			}
			return Json(new { err = "Enter proper value of fields" });
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return Json(new { err = "Internal Server Error" });
		}
	}
	public async Task<IActionResult> ViewHomework()
	{
		var user = await userManager.GetUserAsync(User);
		var subjectList = (from sub in context.Subject
						   join assignSub in context.SubjectAssigned on sub.SubjectId equals assignSub.SubjectId
						   where assignSub.TeacherId == user.Id
						   select new HomeworkListViewModel
						   {
							   SubjectName = sub.SubjectName,
							   SubjectId = sub.SubjectId,
							   ClassId = sub.ClassId,
							   Homeworks = context.Homeworks.Where(h => h.SubectId == sub.SubjectId).ToList()
						   }).OrderBy(s => s.ClassId).ToList();

		return View(subjectList);
	}
	public async Task<IActionResult> HomeworkDetail(int id)
	{
		var user = await userManager.GetUserAsync(User);
		var homeworkDetail = (

			from h in context.Homeworks
			join s in context.Subject on new { SubId = h.SubectId } equals new { SubId = s.SubjectId }
			where h.Id == id
			select new HomeworkDetailViewModel
			{
				Title = h.Title,
				Id = h.Id,
				Description = h.Description,
				DueDate = h.DueDate,
				AssignedHomeworkFilePath = h.AssignedHomeworkFilePath,
				ClassId = h.ClassId,
				SubjectName = s.SubjectName,
				student = (from u in context.Users
						   join h in context.Homeworks on u.ClassId equals h.ClassId
						   join ah in context.SubmittedHomeworks on new { StudentId = u.Id, HomeworkId = h.Id } equals new { StudentId = ah.StudentId, HomeworkId = ah.HomeworkId } into ah_join
						   from ah in ah_join.DefaultIfEmpty()
						   where u.UserTypeId == 1 && u.ClassId == h.ClassId && h.Id == id
						   select new StudentList
						   {
							   GNRNo = u.GNRNo != null ? (int)u.GNRNo : 0,
							   RollNo = u.RollNo != null ? (int)u.RollNo : 0,
							   StudentName = u.FirstName + " " + u.MiddleName + " " + u.LastName,
							   SubmittedFile = ah.UploadedHomeworkPath,
							   SubmittedDate = ah.SubmittedDate,
							   HomeWorkId = ah.HomeworkId,
							   StudentId = u.Id
						   }).OrderBy(h => h.RollNo).ToList()
			}
		).FirstOrDefault();
		return View(homeworkDetail);
	}

	[AllowAnonymous]
	[Authorize(Roles = "Student,Teacher,Admin")]
	public IActionResult HomeworkFile(int id)
	{
		try
		{
			if (User.Identity.IsAuthenticated && (User.IsInRole("Student") || User.IsInRole("Teacher") || User.IsInRole("Admin")))
			{

				var homework = context.Homeworks.Where(s => s.Id == id).FirstOrDefault();
				if (homework != null)
				{
					string path = "wwwroot/" + homework.AssignedHomeworkFilePath;
					byte[] pdf = System.IO.File.ReadAllBytes(path);
					MemoryStream ms = new MemoryStream(pdf);
					return new FileStreamResult(ms, "application/pdf");
				}
				return Json(new { err = "File Not Found !" });
			}
			else
			{
				return RedirectToAction("Login", "Account");
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return Json(new { err = "Internal Server Error !" });
		}
	}

	[AllowAnonymous]
	public IActionResult SubmittedHomeworkFile(int id, string StudentId)
	{
		try
		{
			if (User.Identity.IsAuthenticated && (User.IsInRole("Student") || User.IsInRole("Teacher") || User.IsInRole("Admin")))
			{

				var homework = context.SubmittedHomeworks.Where(s => s.HomeworkId == id && s.StudentId == StudentId).FirstOrDefault();
				if (homework != null)
				{
					string path = "wwwroot/" + homework.UploadedHomeworkPath;
					byte[] pdf = System.IO.File.ReadAllBytes(path);
					MemoryStream ms = new MemoryStream(pdf);
					return new FileStreamResult(ms, "application/pdf");
				}
				return Json(new { err = "File Not Found!" });
			}
			else
			{
				return RedirectToAction("Login", "Account");
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return Json(new { err = "Internal Server Error !" });
		}
	}

}