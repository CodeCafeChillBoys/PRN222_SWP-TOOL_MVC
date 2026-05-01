using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork;
using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
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

        public async Task<TopicResponseDTO> CreateTopicAsync(
      CreateTopicRequestDTO request,
      int teacherId)
        {
            var topic = new Topic
            {
                TopicName = request.TopicName,
                Description = request.Description,
                Requirement = request.Requirement,
                SemesterID = request.SemesterID,
                TeacherID = teacherId,
                MaxGroupCount = request.MaxGroupCount,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.topicRepository.AddAsync(topic);
            await _unitOfWork.SaveChangeAsync();

            return new TopicResponseDTO
            {
                TopicID = topic.TopicID,
                TopicName = topic.TopicName,
                Description = topic.Description,
                Requirement = topic.Requirement,
                SemesterID = topic.SemesterID,
                TeacherID = topic.TeacherID,
                MaxGroupCount = topic.MaxGroupCount,
                CreatedAt = topic.CreatedAt
            };
        }

        public async Task<bool> DeleteTopic(int id)
        {
            var topic = await _unitOfWork.topicRepository.GetByIdAsync(id);

            if (topic == null)
                return false;

            await _unitOfWork.topicRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangeAsync();

            return true;
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

        public async Task<bool> UpdateAsync(UpdateTopicRequestDTO dto)
        {
            var existingTopic = await _unitOfWork.topicRepository.GetByIdAsync(dto.TopicID);

            if (existingTopic == null)
                return false;

            existingTopic.TopicName = dto.TopicName;
            existingTopic.Description = dto.Description;
            existingTopic.Requirement = dto.Requirement;

            await _unitOfWork.topicRepository.UpdateAsync(existingTopic);
            await _unitOfWork.SaveChangeAsync();

            return true;
        }

        // ─── Student side ────────────────────────────────────────────

        public async Task<List<Topic>> GetTopicsWithDetailsAsync()
        {
            // Dùng method typed Include trong TopicRepository — tránh lỗi EF Core Convert expression
            return await _unitOfWork.topicRepository.GetAllWithDetailsAsync();
        }

        public async Task<int?> GetRegisteredTopicIdAsync(int groupId)
        {
            var registrations = await _unitOfWork.topicRegistrationsRepository.GetAllAsync();
            var reg = registrations.FirstOrDefault(r => r.GroupID == groupId);
            return reg?.TopicID;
        }

        public async Task<(bool Success, string Message)> RegisterTopicAsync(int topicId, int groupId)
        {
            // Kiểm tra nhóm chưa đăng ký đề tài nào
            var allRegs = await _unitOfWork.topicRegistrationsRepository.GetAllAsync();

            if (allRegs.Any(r => r.GroupID == groupId))
                return (false, "Nhóm của bạn đã đăng ký đề tài rồi.");

            // Kiểm tra topic còn slot
            var topic = await _unitOfWork.topicRepository.GetByIdAsync(topicId);
            if (topic == null)
                return (false, "Đề tài không tồn tại.");

            int currentCount = allRegs.Count(r => r.TopicID == topicId);
            if (topic.MaxGroupCount.HasValue && currentCount >= topic.MaxGroupCount.Value)
                return (false, "Đề tài này đã đủ số nhóm đăng ký.");

            // Tạo registration
            var reg = new TopicRegistration
            {
                TopicID      = topicId,
                GroupID      = groupId,
                RegisteredAt = DateTime.UtcNow
            };

            await _unitOfWork.topicRegistrationsRepository.AddAsync(reg);
            await _unitOfWork.SaveChangeAsync();

            return (true, "Đăng ký đề tài thành công!");
        }
    }
}
