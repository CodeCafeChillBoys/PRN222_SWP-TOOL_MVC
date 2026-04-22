using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
using System.Text.RegularExpressions;

namespace PRN222_SWP_TOOL_MVC.Models
{
    public class StudentDashboardViewModel
    {
        public string CurrentTab { get; set; }

        //public List<StudentGroup> Groups { get; set; }
        //public List<Topic> Topics { get; set; }
        //public List<Question> Questions { get; set; }

        public TeacherDashboardReuqestDTO teacherDashboardReuqestDTO { get; set; }
    }
}
