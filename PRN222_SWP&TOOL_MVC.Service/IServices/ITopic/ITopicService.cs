using PRN222_SWP_TOOL_MVC.Service.DTO.Response;

namespace PRN222_SWP_TOOL_MVC.Service.IServices.ITopic
{
    public interface ITopicService
    {
        Task<TopicDetailResponseDTO> GetTopicDetailAsync(int id);
    }
}
