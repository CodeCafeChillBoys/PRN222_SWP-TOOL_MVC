using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.ISemesterRepository;
using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork;
using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.UnitOfWork;
using PRN222_SWP_TOOL_MVC.Service.DTO.Response;
using PRN222_SWP_TOOL_MVC.Service.IServices.ISemester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
