using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRN222_SWP_TOOL_MVC.Repository.Entities
{
    public class Topic
    {
        [Key]
        public int TopicID { get; set; }

        [Required]
        [MaxLength(200)]
        public string TopicName { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? Requirement { get; set; }

        [Required]
        public int SemesterID { get; set; }

        [Required]
        public int TeacherID { get; set; }

        public int? MaxGroupCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // ===== Navigation Properties =====
        [ForeignKey("SemesterID")]
        public Semester Semester { get; set; }

        [ForeignKey("TeacherID")]
        public Teacher Teacher { get; set; }


        public ICollection<TopicRegistration> TopicRegistrations { get; set; }
    }
}
