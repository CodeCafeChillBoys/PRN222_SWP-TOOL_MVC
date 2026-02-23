using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PRN222_SWP_TOOL_MVC.Repository.Entities
{
    public class Class
    {
        public int ClassID { get; set; }

        // Gắn với học kỳ
        public int SemesterID { get; set; }
        public Semester Semester { get; set; }

        public string ClassName { get; set; }
        public string ClassCode { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public ICollection<StudentGroup> Groups { get; set; } = new List<StudentGroup>();
    }
}
