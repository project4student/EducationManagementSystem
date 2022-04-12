using System.ComponentModel.DataAnnotations;
using EducationManagementSystem.Models;

namespace EducationManagementSystem.ViewModels;

public class HomeworkListViewModel
{
#nullable disable
	public int ClassId { get; set; }
	public int SubjectId { get; set; }
	public string SubjectName { get; set; }
	public List<Homework> Homeworks { get; set; }

}
