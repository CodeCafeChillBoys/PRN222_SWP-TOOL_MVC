using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PRN222_SWP_TOOL_MVC.Repository.Entities
{
    public class Student
    {
        [Key]
        [ForeignKey("User")]
        public int StudentID { get; set; }

        public string? StudentCode { get; set; }
        public string? Major { get; set; }

        // Navigation
        public User User { get; set; }

    }
}
