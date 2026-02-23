using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PRN222_SWP_TOOL_MVC.Repository.Entities
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        // Tiêu đề câu hỏi (optional)
        public string? Title { get; set; }

        [Required]
        public int TopicID { get; set; }

        [Required]
        public int GroupID { get; set; }

        [Required]
        public int StudentID { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        /// <summary>PENDING | PROCESSING | ANSWERED | CLOSED</summary>
        [Required]
        public string Status { get; set; } = "PENDING";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastReplyAt { get; set; } = DateTime.UtcNow;

        /// <summary>UserID của người reply cuối (null nếu chưa có reply)</summary>
        public int? LastRepliedByUserId { get; set; }

        // ===== Navigation =====

        [ForeignKey("TopicID")]
        public Topic Topic { get; set; } = null!;

        [ForeignKey("GroupID")]
        public StudentGroup StudentGroup { get; set; } = null!;

        [ForeignKey("StudentID")]
        public Student Student { get; set; } = null!;

        [ForeignKey("LastRepliedByUserId")]
        public User? LastRepliedBy { get; set; }

        public ICollection<QuestionMessage> Messages { get; set; } = new List<QuestionMessage>();
    }
}

