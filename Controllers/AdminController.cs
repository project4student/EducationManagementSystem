using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EducationManagementSystem.Models;
using EducationManagementSystem.ViewModels;

namespace EducationManagementSystem.Controllers;

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
            var schedules = new Schedule();
            if (ClassId != 0 && Type != 0)
            {
                schedules.ClassId = ClassId;
                schedules.isExam = Type == 2 ? true : false;
                if (scheduleFile != null)
                {
                    var uploadedFile = "Uploads/LectureScheduls/";
                    uploadedFile += Guid.NewGuid().ToString() + scheduleFile.FileName;
                    var serverFolder = Path.Combine(webHostEnvironment.WebRootPath, uploadedFile);
                    using (var filestream = new FileStream(serverFolder, FileMode.Create))
                    {
                        scheduleFile.CopyTo(filestream);
                    }
                    schedules.FilePath = uploadedFile;
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
        return View();
    }
    public IActionResult GetSubjectList(int classId)
    {
        var Subject = context.Subject.Where(s => s.ClassId == classId).ToList();
        return Json(new { subject = Subject });
    }
    public IActionResult GetSchedule()
    {
        string path = "wwwroot/Uploads/LectureScheduls/180210116017.pdf";
        byte[] pdf = System.IO.File.ReadAllBytes(path);
        System.IO.File.WriteAllBytes(path, pdf);
        MemoryStream ms = new MemoryStream(pdf);
        return new FileStreamResult(ms, "application/pdf");
    }
}