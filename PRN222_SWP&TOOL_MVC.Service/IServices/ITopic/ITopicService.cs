using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
using PRN222_SWP_TOOL_MVC.Service.DTO.Response;
using PRN222_SWP_TOOL_MVC.Repository.Entities;

namespace PRN222_SWP_TOOL_MVC.Service.IServices.ITopic
{
    public interface ITopicService
    {
        Task<TopicDetailResponseDTO> GetTopicDetailAsync(int id);
        Task<TopicResponseDTO> CreateTopicAsync(CreateTopicRequestDTO request, int teacherId);
        Task<bool> UpdateAsync(UpdateTopicRequestDTO dto);
        Task<bool> DeleteTopic(int id);

        // ===== Student side =====
        /// <summary>Lấy tất cả topic kèm teacher + registration count</summary>
        Task<List<Topic>> GetTopicsWithDetailsAsync();
        /// <summary>Lấy TopicID mà nhóm (groupId) đã đăng ký, null nếu chưa</summary>
        Task<int?> GetRegisteredTopicIdAsync(int groupId);
        /// <summary>Trưởng nhóm đăng ký đề tài cho nhóm</summary>
        Task<(bool Success, string Message)> RegisterTopicAsync(int topicId, int groupId);
    }
}
