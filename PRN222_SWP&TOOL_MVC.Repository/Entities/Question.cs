using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PRN222_SWP_TOOL_MVC.Repository.Entities
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TopicID { get; set; }

        [Required]
        public int GroupID { get; set; }

        [Required]
        public int StudentID { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Status { get; set; } = "PENDING";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ===== Navigation =====

        [ForeignKey("TopicID")]
        public Topic Topic { get; set; }

        [ForeignKey("GroupID")]
        public StudentGroup StudentGroup { get; set; }

        [ForeignKey("StudentID")]
        public Student Student { get; set; }
    }
}
