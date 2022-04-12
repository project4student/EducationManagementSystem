namespace EducationManagementSystem.ViewModels;
public class StudentHomeworkDetailViewModel
{
#nullable disable
	public string SubjectName { get; set; }
	public int Id { get; set; }
	public string StudentId { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string AssignFilepath { get; set; }
	public DateTime AssignDate { get; set; }
	public DateTime DueDate { get; set; }
#nullable enable
	public string? SubmittedFilepath { get; set; }
	public DateTime? SubmiteedDate { get; set; }
}