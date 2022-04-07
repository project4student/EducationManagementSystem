using EducationManagementSystem.Models;
using EducationManagementSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

#nullable disable
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddScoped<Email>();

builder.Services.AddDbContextPool<EMSContext>(option =>
{
	option.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection"));
}
);


builder.Services.AddIdentity<Users, IdentityRole>()
.AddEntityFrameworkStores<EMSContext>()
.AddDefaultTokenProviders();

var app = builder.Build();
var env = builder.Environment;
// if (env.IsDevelopment())
// {
// 	app.UseDeveloperExceptionPage();
// }
app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseFileServer();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();
