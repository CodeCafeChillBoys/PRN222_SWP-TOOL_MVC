using PRN222_SWP_TOOL_MVC.Repository.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRN222_SWP_TOOL_MVC.Repository.Entities
{
    public class QuestionMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Required]
        public int SenderUserId { get; set; }

        [Required]
        public SenderRole SenderRole { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ===== Navigation =====
        [ForeignKey("QuestionId")]
        public Question Question { get; set; } = null!;

        [ForeignKey("SenderUserId")]
        public User Sender { get; set; } = null!;
    }
}
