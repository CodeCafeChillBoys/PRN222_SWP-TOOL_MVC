using Microsoft.EntityFrameworkCore;
using PRN222_SWP_TOOL_MVC.Repository.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Semester> Semesters => Set<Semester>();
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<StudentGroup> StudentGroups { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>()
    .HasOne(s => s.User)
    .WithOne(u => u.Student)
    .HasForeignKey<Student>(s => s.StudentID);


        modelBuilder.Entity<StudentGroup>()
      .HasKey(g => g.GroupID);


        // Class - Semester (many classes per semester)
        modelBuilder.Entity<Class>()
            .HasOne(c => c.Semester)
            .WithMany()
            .HasForeignKey(c => c.SemesterID);

        // Group - Class (many groups per class)
        modelBuilder.Entity<StudentGroup>()
            .HasOne(g => g.Class)
            .WithMany(c => c.Groups)
            .HasForeignKey(g => g.ClassID);

        // Group - CreatedByUser
        modelBuilder.Entity<StudentGroup>()
            .HasOne(g => g.CreatedByUser)
            .WithMany()
            .HasForeignKey(g => g.CreatedByUserID)
            .OnDelete(DeleteBehavior.Restrict);

        // GroupMember composite key
        //GroupMember -> Group (many-to-one)
        //Group->Members(one - to - many)
        modelBuilder.Entity<GroupMember>()
            .HasKey(gm => new { gm.GroupID, gm.StudentID });

        modelBuilder.Entity<GroupMember>()
            .HasOne(gm => gm.Group)
            .WithMany(g => g.Members)
            .HasForeignKey(gm => gm.GroupID);

        modelBuilder.Entity<GroupMember>()
            .HasOne(gm => gm.Student)
            .WithMany()
            .HasForeignKey(gm => gm.StudentID);
    }
}