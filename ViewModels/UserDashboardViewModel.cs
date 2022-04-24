using System.ComponentModel.DataAnnotations;

namespace EducationManagementSystem.ViewModels;
public class UserDashboardViewModels
{
#nullable disable
	public string Name { get; set; }
	public string FatherName { get; set; }
	public int Class { get; set; }
	public string Mobile { get; set; }
	public string DateOfBirth { get; set; }
	public string AdmissionDate { get; set; }
	public string Address { get; set; }
	public List<String> Notice { get; set; }
	public string ProfilePicture { get; set; }


}