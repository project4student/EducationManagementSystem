using System.ComponentModel.DataAnnotations;

namespace EducationManagementSystem.Models;

public class SubjectAssigned
{
#nullable disable
	[Required]
	public int Id { get; set; }
	[Required]
	public string TeacherId { get; set; }
	[Required]
	public int SubjectId { get; set; }
	public virtual Users UserNavigation { get; set; }
	public virtual Subject SubjectNavigation { get; set; }
}