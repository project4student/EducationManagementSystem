using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EducationManagementSystem.Models;

public class Users : IdentityUser
{
#nullable disable
	[Required]
	public int UserTypeId { get; set; }
	[Required]
	public string FirstName { get; set; }
	[Required]
	public string MiddleName { get; set; }
	[Required]
	public string LastName { get; set; }
	[Required]
	public string Address { get; set; }
	public string Profile { get; set; }
	[Required]
	public DateTime DateOfBirth { get; set; }
	public virtual ICollection<SubjectAssigned> SubjectAssignedsNavigation { get; set; }
	public virtual ICollection<EncryptedPassword> EncUserNavigation { get; set; }
	public virtual ICollection<SubmittedHomework> SubmittedHomeworkUserNavigation { get; set; }
#nullable enable
	public DateTime? AdmissionDate { get; set; }
	public DateTime? LeavingDate { get; set; }
	public int? RollNo { get; set; }
	public int? GNRNo { get; set; }
	public bool? IsLeaved { get; set; }
	public int? ClassId { get; set; }
	public string? LeavingReason { get; set; }
}