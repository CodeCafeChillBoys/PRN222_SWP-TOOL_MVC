using Microsoft.EntityFrameworkCore;
using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.Entities.QnA;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // ── Existing ────────────────────────────────────────────
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }

    // ── Q&A Domain ──────────────────────────────────────────
    public DbSet<Semester> Semesters { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionMessage> QuestionMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
}
