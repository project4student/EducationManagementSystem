using System.ComponentModel.DataAnnotations;

namespace EducationManagementSystem.Models;
public class Schedule
{
#nullable disable
    [Required]
    public int Id { get; set; }
    [Required]
    public int ClassId { get; set; }
    [Required]
    public string FilePath { get; set; }
    [Required]
    public bool isExam { get; set; }

}