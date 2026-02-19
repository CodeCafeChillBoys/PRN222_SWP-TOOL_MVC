using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork;
using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
using PRN222_SWP_TOOL_MVC.Service.IServices.ITeacher;

namespace PRN222_SWP_TOOL_MVC.Service.Services.TeacherService
{
    public class TeacherService : ITeacherService
    {

        private readonly IUnitOfWork _unitOfWork;

        public TeacherService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<TeacherDashboardReuqestDTO> GetDashboardDataAsync(string tab)
        {
            var questions = await _unitOfWork.questionRepository
                          .GetAllWithIncludeAsync(q => q.Topic);

            var topics = await _unitOfWork.topicRepository
                                .GetAllWithIncludeAsync(t => t.TopicRegistrations,
                                  t => t.Semester);

            return new TeacherDashboardReuqestDTO
            {
                CurrentTab = tab,
                Question = questions.ToList(),
                Topics = topics.ToList()
            };
        }
    }
}
