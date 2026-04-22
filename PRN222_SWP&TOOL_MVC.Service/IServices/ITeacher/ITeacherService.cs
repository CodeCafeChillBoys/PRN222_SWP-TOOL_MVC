using PRN222_SWP_TOOL_MVC.Service.DTO.Response;

namespace PRN222_SWP_TOOL_MVC.Service.IServices.ITeacher
{
    public interface ITeacherService
    {
        Task<TeacherDashboardResponseDTO> GetDashboardDataAsync(string tab);

    }
}
