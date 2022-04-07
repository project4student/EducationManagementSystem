using System.ComponentModel.DataAnnotations;

namespace EducationManagementSystem.Models;

public class Homework
{
#nullable disable
	[Required]
	public int Id { get; set; }
	[Required]
	public string Title { get; set; }
	[Required]
	public string Description { get; set; }
	[Required]
	public string AssignedHomeworkFilePath { get; set; }
	[Required]
	public DateTime CreatedDate { get; set; }
	[Required]
	public DateTime DueDate { get; set; }
	[Required]
	public int ClassId { get; set; }
	[Required]
	public int SubectId { get; set; }
	public virtual Subject SubjectHomeworkNavigation { get; set; }
	public virtual ICollection<SubmittedHomework> HomeworkSubmittedHomeworkNavigation { get; set; }
}