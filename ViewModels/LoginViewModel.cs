using System.ComponentModel.DataAnnotations;

namespace EducationManagementSystem.ViewModels;

public class LoginViewModel
{
#nullable disable
	[Required(ErrorMessage = "Email is Required !")]
	[DataType(DataType.EmailAddress, ErrorMessage = "Enter Proper Email !")]
	public string Email { get; set; }

	[Required(ErrorMessage = "Password is Required !")]
	[RegularExpression(@"^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,16}$", ErrorMessage = "Password must be 6 to 14 length, and contain atleast one capital, one number, one special character !")]
	public string Password { get; set; }
}