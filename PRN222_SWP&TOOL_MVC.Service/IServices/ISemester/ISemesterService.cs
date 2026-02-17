using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Service.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_SWP_TOOL_MVC.Service.IServices.ISemester
{
    public interface ISemesterService
    {
        Task<List<SemesterResponseDTO>> GetAllAsync();
    }
}
