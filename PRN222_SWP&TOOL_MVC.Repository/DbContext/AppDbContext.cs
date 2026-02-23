using Microsoft.EntityFrameworkCore;
using PRN222_SWP_TOOL_MVC.Repository.Entities;
<<<<<<< HEAD
<<<<<<< HEAD
=======
using PRN222_SWP_TOOL_MVC.Repository.Enums;
>>>>>>> 45af78c (fix class AppDBcontext)
=======
using PRN222_SWP_TOOL_MVC.Repository.Entities.QnA;

>>>>>>> b3cf549 (upadate)

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

<<<<<<< HEAD
<<<<<<< HEAD
=======
    // ── Existing ────────────────────────────────────────────
>>>>>>> b3cf549 (upadate)
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }

<<<<<<< HEAD
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
=======
    // ── Q&A Domain ──────────────────────────────────────────
    public DbSet<Semester> Semesters { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionMessage> QuestionMessages { get; set; }
>>>>>>> b3cf549 (upadate)

        // ── Group & Q&A ──────────────────────────────────────────
        public DbSet<StudentGroup> StudentGroups { get; set; } = null!;
        public DbSet<GroupMember> GroupMembers { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<QuestionMessage> QuestionMessages { get; set; } = null!;

<<<<<<< HEAD
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
=======
        // ── Enum → string (Postgres stores as TEXT) ──────────
        modelBuilder.Entity<Question>()
            .Property(q => q.Status)
            .HasConversion<string>();

        modelBuilder.Entity<QuestionMessage>()
            .Property(m => m.SenderRole)
            .HasConversion<string>();

        modelBuilder.Entity<GroupMember>()
            .Property(m => m.RoleInGroup)
            .HasConversion<string>();

        // ── Question: 2 FK tới User — phải tường minh ───────
        modelBuilder.Entity<Question>()
            .HasOne(q => q.CreatedBy)
            .WithMany()
            .HasForeignKey(q => q.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Question>()
            .HasOne(q => q.LastRepliedBy)
            .WithMany()
            .HasForeignKey(q => q.LastRepliedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // ── Topic → Teacher (User) ───────────────────────────
        modelBuilder.Entity<Topic>()
            .HasOne(t => t.Teacher)
            .WithMany()
            .HasForeignKey(t => t.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        // ── QuestionMessage → User ───────────────────────────
        modelBuilder.Entity<QuestionMessage>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // ── Indexes ──────────────────────────────────────────
        // Nhóm xem danh sách câu hỏi của mình, sort theo thời gian
        modelBuilder.Entity<Question>()
            .HasIndex(q => new { q.GroupId, q.CreatedAt })
            .HasDatabaseName("IX_Questions_GroupId_CreatedAt");

        // Teacher xem câu hỏi theo topic, lọc status, sort lastReplyAt
        modelBuilder.Entity<Question>()
            .HasIndex(q => new { q.TopicId, q.Status, q.LastReplyAt })
            .HasDatabaseName("IX_Questions_TopicId_Status_LastReplyAt");

        // GroupMember: tìm nhanh userId trong group
        modelBuilder.Entity<GroupMember>()
            .HasIndex(m => new { m.GroupId, m.UserId })
            .HasDatabaseName("IX_GroupMembers_GroupId_UserId")
            .IsUnique();
    }
>>>>>>> b3cf549 (upadate)
}
