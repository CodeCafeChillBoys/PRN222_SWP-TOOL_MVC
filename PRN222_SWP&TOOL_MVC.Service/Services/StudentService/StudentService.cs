using PRN222_SWP_TOOL_MVC.Repository.Entities;
using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork;
using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
using PRN222_SWP_TOOL_MVC.Service.IServices.IStudent;

namespace PRN222_SWP_TOOL_MVC.Service.Services.StudentService
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<StudentDashboardRequestDTO> GetDashboardAsync(StudentDashboardFilterRequestDTO request)
        {
            // Lấy danh sách group member trong đó có student lấy studentId
            var groupMember = (await _unitOfWork.groupMemberRepository
                .GetAllWithIncludeAsync(gm => gm.Student))
                .FirstOrDefault(gm =>
                    gm.Student != null &&
                    gm.Student.StudentID == request.UserId);


            StudentGroup? group = null;
            // kiểm tra groupMemeber có tồn tại hay ko
            if (groupMember != null)
            {
                // Gán list studentGroup có groupId
                group = (await _unitOfWork.studentGroupRepository
                    .GetAllAsync())
                    .FirstOrDefault(g => g.GroupID == groupMember.GroupID);
            }

            var topics = (await _unitOfWork.topicRepository
                .GetAllWithIncludeAsync(t => t.TopicRegistrations,t => t.Teacher.User))
                .ToList();

            var questions = (await _unitOfWork.questionRepository
                .GetAllAsync())
                .ToList();

            
            //kiểm tra selectTopicId
            int? selectedTopicId = null;


            if (group != null)
            {
                // lấy topicId ra
                selectedTopicId = topics
                    .SelectMany(t => t.TopicRegistrations ?? new List<TopicRegistration>())
                    .Where(r => r.GroupID == group.GroupID)
                    .Select(r => (int?)r.TopicID)
                    .FirstOrDefault(); //lấy cái đầu tiên 
            }

            return new StudentDashboardRequestDTO
            {
                CurrentTab = request.Tab,
                Topics = topics,
                Questions = questions,
                SelectedTopicId = selectedTopicId
            };
        }
        public async Task<ReturnData<bool>> RegisterTopic(RegisterTopicRequestDTO request)
        {
            var result = new ReturnData<bool>();
            var group = await _unitOfWork.studentGroupRepository.GetAsync(g => g.CreatedByUserID == request.UserId);

            if (group == null)
            {
                result.Success = false;
                result.ResponseMessage = "Bạn chưa có nhóm.";
                return result;
            }

            bool alreadyRegistered = await _unitOfWork.topicRegistrationsRepository.ExistsAsync(r => r.GroupID == group.GroupID);
            if (alreadyRegistered)
            {
                result.Success = false;
                result.ResponseMessage = "Nhóm đã đăng ký chủ đề rồi.";
                return result;
            }

            var topic = await _unitOfWork.topicRepository.GetAsync(r => r.TopicID == request.TopicId);
            if (topic == null)
            {
                result.Success = false;
                result.ResponseMessage = "Không tìm thấy chủ đề.";
                return result;
            }


            int currentCount = await _unitOfWork.topicRepository.CountAsync(r => r.TopicID == request.TopicId);

            if (topic.MaxGroupCount.HasValue &&
                currentCount >= topic.MaxGroupCount.Value)
            {
                result.Success = false;
                result.ResponseMessage = "Chủ đề đã hết slot.";
                return result;
            }


            var registration = new TopicRegistration
            {
                TopicID = request.TopicId,
                GroupID = group.GroupID,
                RegisteredAt = DateTime.Now
            };

            await _unitOfWork.topicRegistrationsRepository.AddAsync(registration);
            await _unitOfWork.SaveChangeAsync();

            result.Success = true;
            result.Data = true;
            result.ResponseMessage = "Đăng ký thành công!";
            return result;
        }
    }
}
