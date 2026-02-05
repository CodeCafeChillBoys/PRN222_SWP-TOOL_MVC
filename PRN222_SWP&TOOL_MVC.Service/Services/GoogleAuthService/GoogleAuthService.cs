using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork;
using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
using PRN222_SWP_TOOL_MVC.Service.IServices.IAuthentication;
using PRN222_SWP_TOOL_MVC.Service.MessageHelper;

namespace PRN222_SWP_TOOL_MVC.Service.Services.GoogleAuthService
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GoogleAuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ReturnData<User>> CreateGoogleUserAsync(SelectRoleRequestDTO selectRoleRequestDTO)
        {

            var result = new ReturnData<User>();

            if (string.IsNullOrEmpty(selectRoleRequestDTO.email))
            {
                result.Success = false;
                result.ResponseMessage = MessageFails.EmailInvalid;
                return result;
            }

            var role = await _unitOfWork.roleRepository
                .GetByIdAsync(selectRoleRequestDTO.roleId);

            if (role == null)
            {
                result.Success = false;
                result.ResponseMessage = MessageFails.RoleInvalid;
                return result;
            }

            var user = new User
            {
                Email = selectRoleRequestDTO.email,
                FullName = selectRoleRequestDTO.name,
                RoleID = selectRoleRequestDTO.roleId,
                Provider = "Google",
                ProviderUserId = selectRoleRequestDTO.ProviderUserId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.userRepository.AddAsync(user);
            await _unitOfWork.SaveChangeAsync();

            result.Success = true;
            result.Data = user;
            return result;
        }

        public async Task<User?> LoginWithGoogleAsync(LoginRequestDTO info)
        {
            return await _unitOfWork.userRepository.FindWitInclude(u =>
                u.Email == info.Email &&
                u.Provider == "Google", u => u.Role);
        }
    }
}
