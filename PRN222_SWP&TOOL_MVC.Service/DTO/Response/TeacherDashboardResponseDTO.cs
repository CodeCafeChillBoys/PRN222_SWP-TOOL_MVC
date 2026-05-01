using PRN222_SWP_TOOL_MVC.Repository.Entities;

namespace PRN222_SWP_TOOL_MVC.Service.DTO.Response
{
    public class TeacherDashboardResponseDTO
    {
        public string CurrentTab { get; set; }
        public List<Topic> Topics { get; set; }
        public List<Question> Question { get; set; }
        public List<SemesterResponseDTO> Semesters { get; set; }
    }
}
