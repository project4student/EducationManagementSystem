using System.ComponentModel.DataAnnotations;
using EducationManagementSystem.Models;

namespace EducationManagementSystem.ViewModels;

public class HomeworkDetailViewModel
{
#nullable disable
	public string Title { get; set; }
	public int Id { get; set; }
	public string Description { get; set; }
	public int ClassId { get; set; }
	public string SubjectName { get; set; }
	public DateTime AssignDate { get; set; }
	public DateTime DueDate { get; set; }
	public string AssignedHomeworkFilePath { get; set; }
	public List<StudentList> student { get; set; }
#nullable enable
	public int? SubId { get; set; }

}

public class StudentList
{
	public int GNRNo { get; set; }
	public int RollNo { get; set; }
	public string StudentName { get; set; }
	public bool isSubmited { get; set; }
#nullable enable
	public int? HomeWorkId { get; set; }
	public string? SubmittedFile { get; set; }
	public DateTime? SubmittedDate { get; set; }
}