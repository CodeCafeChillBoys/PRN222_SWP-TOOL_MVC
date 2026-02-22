using PRN222_SWP_TOOL_MVC.Service.DTO.Request;

namespace PRN222_SWP_TOOL_MVC.Service.IServices.IStudent
{
    public interface IStudentService
    {
        Task<StudentDashboardRequestDTO> GetDashboardAsync(string tab);

    }
}
