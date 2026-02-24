using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork;
using PRN222_SWP_TOOL_MVC.Service.DTO.Response;
using PRN222_SWP_TOOL_MVC.Service.IServices.ISemester;

namespace PRN222_SWP_TOOL_MVC.Service.Services.SemesterService
{
    public class SemesterService : ISemesterService
    {
        private readonly IUnitOfWork _iUnitOfWork;

        public SemesterService(IUnitOfWork unitOfWork)
        {
            _iUnitOfWork = unitOfWork;
        }

        public async Task<List<SemesterResponseDTO>> GetAllAsync()
        {  
            var data = await _iUnitOfWork.semesterRepository.GetAllAsync();
            return data.Select(s => new SemesterResponseDTO
            {
                SemesterID = s.SemesterID,
                SemesterName = s.SemesterName,
                StartDate = s.StartDate,
                EndDate = s.EndDate
            }).ToList(); ;
        }
    }
}
