using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork;
using PRN222_SWP_TOOL_MVC.Service.DTO.Response;
using PRN222_SWP_TOOL_MVC.Service.IServices.ITopic;

namespace PRN222_SWP_TOOL_MVC.Service.Services.TopicService
{
    public class TopicService : ITopicService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TopicService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TopicDetailResponseDTO> GetTopicDetailAsync(int id)
        {
            var topic = await _unitOfWork.topicRepository.GetByIdAsync(id);

            if (topic == null)
            {
                return new TopicDetailResponseDTO
                {
                    Success = false
                };
            }
            return new TopicDetailResponseDTO
            {
                Success = true,
                TopicID = topic.TopicID,
                TopicName = topic.TopicName,
                Description = topic.Description,
                Requirement = topic.Requirement
            };
        }
    }
}
