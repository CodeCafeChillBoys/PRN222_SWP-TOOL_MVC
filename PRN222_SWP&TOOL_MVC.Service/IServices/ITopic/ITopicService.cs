using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
using PRN222_SWP_TOOL_MVC.Service.DTO.Response;

namespace PRN222_SWP_TOOL_MVC.Service.IServices.ITopic
{
    public interface ITopicService
    {
        Task<TopicDetailResponseDTO> GetTopicDetailAsync(int id);
        Task<Topic?>               GetByIdAsync(int id);
        Task<List<Topic>>          GetAllTopicsAsync();

        public Task<TopicResponseDTO> CreateTopicAsync(CreateTopicRequestDTO request, int teacherId);

        Task<bool> UpdateAsync(UpdateTopicRequestDTO dto);
        Task<bool> DeleteTopic(int id);
    }
}
