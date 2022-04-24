using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace EducationManagementSystem.Models;

public class EMSContext : IdentityDbContext<Users>
{
	public EMSContext(DbContextOptions options) : base(options)
	{

	}
#nullable disable
	public DbSet<Users> User { get; set; }
	public DbSet<Subject> Subject { get; set; }
	public DbSet<SubjectAssigned> SubjectAssigned { get; set; }
	public DbSet<EncryptedPassword> EncryptedPasswords { get; set; }
	public DbSet<Schedule> Schedules { get; set; }
	public DbSet<SubmittedHomework> SubmittedHomeworks { get; set; }
	public DbSet<Homework> Homeworks { get; set; }
	public DbSet<Notice> Notices { get; set; }
	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<Homework>(entity =>
		{
			entity.HasKey(h => h.Id);
			entity.HasOne(h => h.SubjectHomeworkNavigation)
			.WithMany(s => s.SubjectHomeworkNavigation)
			.HasForeignKey(h => h.SubectId)
			.OnDelete(DeleteBehavior.Cascade);
		});

		builder.Entity<SubmittedHomework>(entity =>
		{
			entity.HasKey(sh => sh.Id);

			entity.HasOne(sh => sh.HomeworkSubmittedHomeworkNavigation)
			.WithMany(h => h.HomeworkSubmittedHomeworkNavigation)
			.HasForeignKey(sh => sh.HomeworkId)
			.OnDelete(DeleteBehavior.Cascade);

			entity.HasOne(sh => sh.SubmittedHomeworkUserNavigation)
			.WithMany(u => u.SubmittedHomeworkUserNavigation)
			.HasForeignKey(sh => sh.StudentId)
			.OnDelete(DeleteBehavior.Cascade);
		});

		builder.Entity<Subject>(entity =>
		{
			entity.HasKey(s => s.SubjectId);
		});

		builder.Entity<Users>(entity =>
		{
			entity.HasKey(u => u.Id);
		});

		builder.Entity<SubjectAssigned>(entity =>
		{
			entity.HasKey(sa => sa.Id);
			entity.HasOne(sa => sa.SubjectNavigation)
			.WithMany(s => s.SubjectAssignedNavigation)
			.HasForeignKey(sa => sa.SubjectId)
			.OnDelete(DeleteBehavior.Cascade);

			entity.HasOne(sa => sa.UserNavigation)
			.WithMany(u => u.SubjectAssignedsNavigation)
			.HasForeignKey(sa => sa.TeacherId)
			.OnDelete(DeleteBehavior.Cascade);
		});
		builder.Entity<EncryptedPassword>(entity =>
		{
			entity.HasKey(ep => ep.Id);
			entity.HasOne(ep => ep.EncUserNavigation)
			.WithMany(u => u.EncUserNavigation)
			.HasForeignKey(ep => ep.UserId)
			.OnDelete(DeleteBehavior.Cascade);
		});

		builder.Entity<Schedule>(entity =>
		{
			entity.HasKey(s => s.Id);
		});

		builder.Entity<Notice>(entity =>
		{
			entity.HasKey(n => n.Id);
		});

	}
}