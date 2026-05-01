using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IGenericRepository;

namespace PRN222_SWP_TOOL_MVC.Repository.IRepositories.ITopRepositroy
{
    public interface ITopicRepository : IGenericRepository<Topic>
    {
        /// <summary>Lấy tất cả topic kèm Teacher, TopicRegistrations — dùng typed Include để EF Core không lỗi</summary>
        Task<List<Topic>> GetAllWithDetailsAsync();
    }
}
