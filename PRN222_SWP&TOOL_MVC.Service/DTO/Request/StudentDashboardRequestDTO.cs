using PRN222_SWP_TOOL_MVC.Repository.Entities;

namespace PRN222_SWP_TOOL_MVC.Service.DTO.Request
{
    public class StudentDashboardRequestDTO
    {
        public string CurrentTab { get; set; }

        public List<Topic> Topics { get; set; }
        public List<Question> Questions { get; set; }
        public int? SelectedTopicId { get; set; }
    }
}
