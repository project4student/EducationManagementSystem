using System.ComponentModel.DataAnnotations;
using EducationManagementSystem.Models;

namespace EducationManagementSystem.ViewModels;
public class AssignSubjectViewModel
{
#nullable disable
    [Required]
    public string TeacherId { get; set; }
    [Required]
    public int SubjectId { get; set; }
    [Required]
    public int ClassId { get; set; }

#nullable enable
    public List<Users>? Teacher { get; set; }
}