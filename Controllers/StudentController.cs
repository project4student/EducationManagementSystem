using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EducationManagementSystem.Models;
using EducationManagementSystem.ViewModels;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EducationManagementSystem.Controllers;

[Authorize(Roles = "Student")]
public class StudentController : Controller
{
	private readonly UserManager<Users> userManager;
	private readonly RoleManager<IdentityRole> roleManager;
	private readonly EMSContext context;
	private readonly SignInManager<Users> signinManager;
	private readonly IWebHostEnvironment webHostEnvironment;

	public StudentController(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, EMSContext context, SignInManager<Users> signinManager, IWebHostEnvironment webHostEnvironment)
	{
		this.userManager = userManager;
		this.roleManager = roleManager;
		this.context = context;
		this.signinManager = signinManager;
		this.webHostEnvironment = webHostEnvironment;

	}
	public async Task<IActionResult> Index()
	{
		var user = await userManager.GetUserAsync(User);
		return View(user);
	}
	public IActionResult ViewSchedule(int id)
	{
		try
		{
			var schedule = context.Schedules.Where(s => s.Id == id).FirstOrDefault();
			if (schedule != null)
			{

				string path = "wwwroot/" + schedule.FilePath;
				byte[] pdf = System.IO.File.ReadAllBytes(path);
				System.IO.File.WriteAllBytes(path, pdf);
				MemoryStream ms = new MemoryStream(pdf);
				return new FileStreamResult(ms, "application/pdf");
			}
			return Json(new { err = "File Not Found !" });

		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return Json(new { err = "Internal Server Error !" });
		}
	}
	public async Task<IActionResult> IdCard()
	{
		var user = await userManager.GetUserAsync(User);
		return View(user);
	}
	public IActionResult Homework()
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		var homeworkDetail = (
			from u in context.Users
			join h in context.Homeworks on u.ClassId equals h.ClassId
			join s in context.Subject on new { SubId = h.SubectId } equals new { SubId = s.SubjectId }
			join sh in context.SubmittedHomeworks on new { StudentId = u.Id, HomeworkId = h.Id } equals new { StudentId = sh.StudentId, HomeworkId = sh.HomeworkId } into sh_join
			from sh in sh_join.DefaultIfEmpty()
			where u.Id == userId
			select new HomeworkDetailViewModel
			{
				SubjectName = s.SubjectName,
				Title = h.Title,
				Id = h.Id,
				AssignDate = h.CreatedDate,
				DueDate = h.DueDate,
				student = new List<StudentList>{
					new StudentList(){
						SubmittedDate=sh.SubmittedDate
					}
				}
			}
		).ToList();
		return View(homeworkDetail);
	}
	public IActionResult HomeworkDetail(int Id, string SubjectName, bool isSubmit)
	{
		var userId = isSubmit ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
		var homework = (
			from h in context.Homeworks
			join sh in context.SubmittedHomeworks on h.Id equals sh.HomeworkId into sh_join
			from sh in sh_join.DefaultIfEmpty()
			where h.Id == Id
			select new StudentHomeworkDetailViewModel
			{
				SubjectName = SubjectName,
				Title = h.Title,
				StudentId = sh.StudentId,
				Description = h.Description,
				AssignFilepath = h.AssignedHomeworkFilePath,
				AssignDate = h.CreatedDate,
				DueDate = h.DueDate,
				SubmiteedDate = sh.SubmittedDate,
				SubmittedFilepath = sh.UploadedHomeworkPath
			}).Where(h => h.StudentId == userId).FirstOrDefault();

		return View(homework);
	}
	[HttpPost]
	public IActionResult HomeworkDetail(IFormFile HomeworkFile, int HomeworkId)
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		try
		{
			var SubmittedHomework = new SubmittedHomework()
			{
				StudentId = userId,
				SubmittedDate = DateTime.Now,
				HomeworkId = HomeworkId
			};
			if (HomeworkFile != null)
			{
				var uploadedFile = "Uploads/SubmittedHomework/";
				uploadedFile += Guid.NewGuid().ToString() + "_" + HomeworkFile.FileName;
				var serverFolder = Path.Combine(webHostEnvironment.WebRootPath, uploadedFile);
				using (var filestream = new FileStream(serverFolder, FileMode.Create))
				{
					HomeworkFile.CopyTo(filestream);
				}
				SubmittedHomework.UploadedHomeworkPath = uploadedFile;
				context.SubmittedHomeworks.Add(SubmittedHomework);
				context.SaveChanges();
				return Json(new { success = "Homework Submitted Successfuly" });
			}
			else
			{
				return Json(new { err = "Select proper file" });
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return Json(new { err = "Internal server Error" });
		}
	}
}
