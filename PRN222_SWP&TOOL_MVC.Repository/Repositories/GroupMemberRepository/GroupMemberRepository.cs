using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IGroupMemberRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.GenericRepository;

namespace PRN222_SWP_TOOL_MVC.Repository.Repositories.GroupMemberRepository
{
    public class GroupMemberRepository : GenericRepository<GroupMember>, IGroupMemberRepository
    {
        public GroupMemberRepository(AppDbContext context) : base(context)
        {
        }
    }
}
