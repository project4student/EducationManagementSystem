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
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

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
    }
}