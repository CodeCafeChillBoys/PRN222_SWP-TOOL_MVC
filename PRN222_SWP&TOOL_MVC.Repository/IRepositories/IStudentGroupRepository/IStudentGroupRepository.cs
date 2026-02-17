using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PRN222_SWP_TOOL_MVC.Repository.IRepositories.IGroupRepository
{
    public interface IStudentGroupRepository : IGenericRepository<StudentGroup>
    {
        Task<StudentGroup?> GetGroupWithMembersAsync(int groupId);
        Task<int> CountMembersAsync(int groupId);
    }
}
