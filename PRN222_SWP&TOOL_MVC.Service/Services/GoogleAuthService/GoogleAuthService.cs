using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork;
using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
using PRN222_SWP_TOOL_MVC.Service.Enum;
using PRN222_SWP_TOOL_MVC.Service.IServices.IAuthentication;

namespace PRN222_SWP_TOOL_MVC.Service.Services.GoogleAuthService
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GoogleAuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<User> LoginWithGoogleAsync(LoginRequestDTO info)
        {
            var user = await _unitOfWork.userRepository.GetAsync(s => s.Email == info.Email);
            if (user == null)
            {
                user = new User
                {
                    Email = info.Email,
                    FullName = info.FullName,
                    RoleID =(int)EnumRole.STUDENT,
                    CreatedAt = DateTime.UtcNow,
                    Provider = "GOOGLE",
                    ProviderUserId = info.ProviderUserId,

                };
                await _unitOfWork.userRepository.AddAsync(user);
                await _unitOfWork.SaveChangeAsync();
            }

            return user;
        }
    }
}
