using System.ComponentModel.DataAnnotations;

public class CreateAdminViewModel
{
#nullable disable
	[Required]
	public string FirstName { get; set; }
	[Required]
	public string LastName { get; set; }
	[Required]
	public string MiddleName { get; set; }
	[Required]
	public string Address { get; set; }
	[Required]
	public string DOB { get; set; }
	[Required]
	public string Email { get; set; }
	[Required]
	public string PhoneNumber { get; set; }
	[Required]
	public string Password { get; set; }
}