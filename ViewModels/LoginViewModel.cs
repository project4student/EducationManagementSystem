using System.ComponentModel.DataAnnotations;

namespace EducationManagementSystem.ViewModels;

public class LoginViewModel
{
#nullable disable
	[Required(ErrorMessage = "Email is Required !")]
	[DataType(DataType.EmailAddress, ErrorMessage = "Enter Proper Email !")]
	public string Email { get; set; }

	[Required(ErrorMessage = "Password is Required !")]
	public string Password { get; set; }
}