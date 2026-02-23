using Microsoft.EntityFrameworkCore;
using PRN222_SWP_TOOL_MVC.Repository.Entities;
<<<<<<< HEAD
=======
using PRN222_SWP_TOOL_MVC.Repository.Enums;
>>>>>>> 45af78c (fix class AppDBcontext)

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

<<<<<<< HEAD
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }

=======
    protected AppDbContext() { }

        // ── Core ─────────────────────────────────────────────────
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;

        // ── Academic ─────────────────────────────────────────────
        public DbSet<Semester> Semesters { get; set; } = null!;
        public DbSet<Class> Classes { get; set; } = null!;
        public DbSet<Topic> Topics { get; set; } = null!;
        public DbSet<TopicRegistration> TopicRegistrations { get; set; } = null!;

        // ── Group & Q&A ──────────────────────────────────────────
        public DbSet<StudentGroup> StudentGroups { get; set; } = null!;
        public DbSet<GroupMember> GroupMembers { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<QuestionMessage> QuestionMessages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Question: Status stored as string ─────────────────
            modelBuilder.Entity<Question>()
                .Property(q => q.Status)
                .HasConversion<string>();

            // ── GroupMember: composite PK ─────────────────────────
            modelBuilder.Entity<GroupMember>()
                .HasKey(m => new { m.GroupID, m.StudentID });

            // ── GroupMember → StudentGroup ────────────────────────
            modelBuilder.Entity<GroupMember>()
                .HasOne(m => m.Group)
                .WithMany(g => g.Members)
                .HasForeignKey(m => m.GroupID)
                .OnDelete(DeleteBehavior.Cascade);

            // ── Topic → Teacher ───────────────────────────────────
            modelBuilder.Entity<Topic>()
                .HasOne(t => t.Teacher)
                .WithMany()
                .HasForeignKey(t => t.TeacherID)
                .OnDelete(DeleteBehavior.Restrict);

            // ── Question → Topic ──────────────────────────────────
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Topic)
                .WithMany()
                .HasForeignKey(q => q.TopicID)
                .OnDelete(DeleteBehavior.Restrict);

            // ── Question → StudentGroup ───────────────────────────
            modelBuilder.Entity<Question>()
                .HasOne(q => q.StudentGroup)
                .WithMany()
                .HasForeignKey(q => q.GroupID)
                .OnDelete(DeleteBehavior.Restrict);

            // ── Question → Student ────────────────────────────────
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Student)
                .WithMany()
                .HasForeignKey(q => q.StudentID)
                .OnDelete(DeleteBehavior.Restrict);

            // ── StudentGroup → User (CreatedBy) ───────────────────
            modelBuilder.Entity<StudentGroup>()
                .HasOne(g => g.CreatedByUser)
                .WithMany()
                .HasForeignKey(g => g.CreatedByUserID)
                .OnDelete(DeleteBehavior.Restrict);

            // ── Question → User (LastRepliedBy) ──────────────────
            modelBuilder.Entity<Question>()
                .HasOne(q => q.LastRepliedBy)
                .WithMany()
                .HasForeignKey(q => q.LastRepliedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ── QuestionMessage ───────────────────────────────────
            modelBuilder.Entity<QuestionMessage>()
                .Property(m => m.SenderRole)
                .HasConversion<string>();

            modelBuilder.Entity<QuestionMessage>()
                .HasOne(m => m.Question)
                .WithMany(q => q.Messages)
                .HasForeignKey(m => m.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuestionMessage>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ── Indexes ───────────────────────────────────────────
            modelBuilder.Entity<Question>()
                .HasIndex(q => new { q.GroupID, q.CreatedAt })
                .HasDatabaseName("IX_Questions_GroupID_CreatedAt");

            modelBuilder.Entity<Question>()
                .HasIndex(q => new { q.TopicID, q.Status })
                .HasDatabaseName("IX_Questions_TopicID_Status");

            modelBuilder.Entity<GroupMember>()
                .HasIndex(m => new { m.GroupID, m.StudentID })
                .HasDatabaseName("IX_GroupMembers_GroupID_StudentID")
                .IsUnique();
        }
>>>>>>> 45af78c (fix class AppDBcontext)
}
