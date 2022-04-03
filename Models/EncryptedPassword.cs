using System.ComponentModel.DataAnnotations;

namespace EducationManagementSystem.Models;
public class EncryptedPassword
{
#nullable disable
    [Required]
    public int Id { get; set; }
    [Required]
    public string EncPassword { get; set; }
    [Required]
    public string UserId { get; set; }
    public virtual Users EncUserNavigation { get; set; }
}