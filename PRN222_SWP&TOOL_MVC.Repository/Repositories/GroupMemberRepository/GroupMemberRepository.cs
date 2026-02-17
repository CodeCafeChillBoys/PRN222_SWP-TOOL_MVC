using Microsoft.EntityFrameworkCore;
using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IGroupMemberRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_SWP_TOOL_MVC.Repository.Repositories.GroupMemberRepository
{
    public class GroupMemberRepository : GenericRepository<GroupMember>, IGroupMemberRepository
    {
        public GroupMemberRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<int> CountByGroupAsync(int groupId)
        {
            return await _context.GroupMembers.CountAsync(x => x.GroupID == groupId);
        }

        public async Task<GroupMember?> GetMemberAsync(int groupId, int studentId)
        {
            return await _context.GroupMembers
                .FirstOrDefaultAsync(x => x.GroupID == groupId && x.StudentID == studentId);
        }

        public async Task<bool> IsMemberAsync(int groupId, int studentId)
        {
            return await _context.GroupMembers
                .AnyAsync(x => x.GroupID == groupId && x.StudentID == studentId);
        }

        public Task RemoveAsync(GroupMember member)
        {
            _context.GroupMembers.Remove(member);
            return Task.CompletedTask;
        }
    }
}
