using System.ComponentModel.DataAnnotations;

namespace EducationManagementSystem.Models;

public class Subject
{
#nullable disable
	[Required]
	public int SubjectId { get; set; }
	[Required]
	public string SubjectName { get; set; }
	[Required]
	public int ClassId { get; set; }

	public virtual ICollection<SubjectAssigned> SubjectAssignedNavigation { get; set; }
	public virtual ICollection<Homework> SubjectHomeworkNavigation { get; set; }

}