using PRN222_SWP_TOOL_MVC.Repository.Entities;

namespace PRN222_SWP_TOOL_MVC.Models
{
    public class TeacherDashboardViewModel
    {
        public string CurrentTab { get; set; }
        public List<Topic> Topics { get; set; }
        public List<Question> Question { get; set; }
    }
}
