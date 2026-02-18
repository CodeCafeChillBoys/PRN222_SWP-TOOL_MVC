using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PRN222_SWP_TOOL_MVC.Repository.Entities
{
    public class TopicRegistration
    {
        [Key]
        public int RegistrationID { get; set; }

        [Required]
        public int TopicID { get; set; }

        [Required]
        public int GroupID { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.Now;

        // ===== Navigation Properties =====
        [ForeignKey("TopicID")]
        public Topic Topic { get; set; }

        [ForeignKey("GroupID")]
        public StudentGroup StudentGroup { get; set; }


    }
}
