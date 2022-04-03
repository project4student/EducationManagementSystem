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

    public AdminController(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, EMSContext context, SignInManager<Users> signinManager)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.context = context;
        this.signinManager = signinManager;

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
    public IActionResult GetSubjectList(int classId)
    {
        var Subject = context.Subject.Where(s => s.ClassId == classId).ToList();
        return Json(new { subject = Subject });
    }
}