using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PRN222_SWP_TOOL_MVC.Repository.Entities
{
    public class Teacher
    {
        [Key]
        [ForeignKey("User")]
        public int TeacherID { get; set; }

        public string? Department { get; set; }

        // Navigation
        public User User { get; set; }
    }
}
