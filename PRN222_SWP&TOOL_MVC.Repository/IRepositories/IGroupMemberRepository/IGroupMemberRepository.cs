using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_SWP_TOOL_MVC.Repository.IRepositories.IGroupMemberRepository
{
    public interface IGroupMemberRepository : IGenericRepository<GroupMember>
    {
        Task<bool> IsMemberAsync(int groupId, int studentId);
        Task<GroupMember?> GetMemberAsync(int groupId, int studentId);
        Task<int> CountByGroupAsync(int groupId);
        Task RemoveAsync(GroupMember member);
    }
}
