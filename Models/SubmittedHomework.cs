using System.ComponentModel.DataAnnotations;

namespace EducationManagementSystem.Models;

public class SubmittedHomework
{
#nullable disable
	[Required]
	public int Id { get; set; }
	[Required]
	public string StudentId { get; set; }
	[Required]
	public int HomeworkId { get; set; }
	[Required]
	public string UploadedHomeworkPath { get; set; }
	[Required]
	public DateTime SubmittedDate { get; set; }
	public virtual Homework HomeworkSubmittedHomeworkNavigation { get; set; }
	public virtual Users SubmittedHomeworkUserNavigation { get; set; }
}