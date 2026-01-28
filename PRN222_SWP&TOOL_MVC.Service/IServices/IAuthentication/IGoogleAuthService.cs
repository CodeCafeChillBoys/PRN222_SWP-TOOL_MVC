using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Service.DTO.Request;

namespace PRN222_SWP_TOOL_MVC.Service.IServices.IAuthentication
{
    public interface IGoogleAuthService
    {
        Task<User> LoginWithGoogleAsync(LoginRequestDTO info);
    }
}
