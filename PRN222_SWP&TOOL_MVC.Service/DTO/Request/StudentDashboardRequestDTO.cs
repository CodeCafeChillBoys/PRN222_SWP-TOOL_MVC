using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Service.Models;

namespace PRN222_SWP_TOOL_MVC.Service.DTO.Request
{
    public class StudentDashboardRequestDTO
    {
        public string CurrentTab { get; set; }

        public GroupDetailsViewModel Group { get; set; } = new();
        public List<Topic> Topics { get; set; }
        public List<Question> Questions { get; set; }
    }
}
