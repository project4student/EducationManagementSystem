using System.ComponentModel.DataAnnotations;

namespace EducationManagementSystem.ViewModels;

public class UserViewModels
{
#nullable disable
	[Required]
	public int UserTypeId { get; set; }
	[Required]
	public string FirstName { get; set; }
	[Required]
	public string MiddleName { get; set; }
	[Required]
	public string LastName { get; set; }
	[Required]
	public string Address { get; set; }
	[Required]
	public DateTime DateOfBirth { get; set; }
	[Required]
	[RegularExpression(@"^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,14}$", ErrorMessage = "Password must be 6 to 14 length, and contain atleast one capital, one number, one special character !")]
	public string Password { get; set; }
	[Required]
	public string Email { get; set; }
	[Required]
	[MinLength(10, ErrorMessage = "Mobile Number must be 10 digit")]
	public string PhoneNumber { get; set; }
	public IFormFile ProfilePicture { get; set; }

#nullable enable
	public int? ClassId { get; set; }
	public int? RollNumber { get; set; }
}