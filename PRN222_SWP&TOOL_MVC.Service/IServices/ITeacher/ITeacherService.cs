using PRN222_SWP_TOOL_MVC.Service.DTO.Request;

namespace PRN222_SWP_TOOL_MVC.Service.IServices.ITeacher
{
    public interface ITeacherService
    {
        Task<TeacherDashboardReuqestDTO> GetDashboardDataAsync(string tab);

    }
}
