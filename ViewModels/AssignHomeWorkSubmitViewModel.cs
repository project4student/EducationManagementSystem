using System.ComponentModel.DataAnnotations;

namespace EducationManagementSystem.ViewModels;

public class AssignHomeWorkSubmitViewModel
{
#nullable disable
	[Required]
	public int SubjectId { get; set; }
	[Required]
	public string Title { get; set; }
	[Required]
	public string Description { get; set; }
	[Required]
	public int ClassId { get; set; }
	[Required]
	public string StartDate { get; set; }
	[Required]
	public string DueDate { get; set; }
	[Required]
	public IFormFile HomeworkFile { get; set; }
#nullable enable
	public List<SubjectList>? SubjectLists { get; set; }

}
public class SubjectList
{
#nullable disable
	public string SubjectName { get; set; }
	public int SubjectId { get; set; }
	public int ClassId { get; set; }
}