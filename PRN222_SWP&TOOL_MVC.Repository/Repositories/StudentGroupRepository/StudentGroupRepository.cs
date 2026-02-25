using Microsoft.EntityFrameworkCore;
using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IGroupRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PRN222_SWP_TOOL_MVC.Repository.Repositories.GroupRepository
{
    public class StudentGroupRepository : GenericRepository<StudentGroup>, IStudentGroupRepository
    {
        public StudentGroupRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<int> CountMembersAsync(int groupId)
        {
            return await _context.GroupMembers.CountAsync(x => x.GroupID == groupId);
        }

        public async Task<StudentGroup?> GetGroupWithMembersAsync(int groupId)
        {
            return await _context.StudentGroups
           .Include(g => g.Members)
           .ThenInclude(m => m.Student)
           .ThenInclude(s => s.User) 
           .FirstOrDefaultAsync(g => g.GroupID == groupId);
        }

        public async Task<StudentGroup?> GetByInviteCodeAsync(string inviteCode)
        {
            return await _context.StudentGroups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.InviteCode == inviteCode.Trim().ToUpper());
        }
    }
}
