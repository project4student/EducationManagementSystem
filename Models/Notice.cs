using System.ComponentModel.DataAnnotations;

namespace EducationManagementSystem.Models;

public class Notice
{
#nullable disable
	public int Id { get; set; }
	public int ClassId { get; set; }
	public string details { get; set; }
	public DateTime CreatedDate { get; set; }
}