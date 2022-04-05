using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EducationManagementSystem.Models;
using EducationManagementSystem.ViewModels;
using Microsoft.Extensions.FileProviders;

namespace EducationManagementSystem.Controllers;

public class StudentController : Controller
{
    private readonly UserManager<Users> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly EMSContext context;
    private readonly SignInManager<Users> signinManager;

    public StudentController(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, EMSContext context, SignInManager<Users> signinManager)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.context = context;
        this.signinManager = signinManager;

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
                // await Response.SendFileAsync(path);
                // await Response.CompleteAsync();
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
    public IActionResult IdCard()
    {
        return View();
    }
}