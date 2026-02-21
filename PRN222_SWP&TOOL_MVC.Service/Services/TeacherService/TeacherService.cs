using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork;
using PRN222_SWP_TOOL_MVC.Service.DTO.Request;
using PRN222_SWP_TOOL_MVC.Service.DTO.Response;
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
                .GetAllWithIncludeAsync(
                    t => t.TopicRegistrations,
                    t => t.Semester
                );

            var semesters = await _unitOfWork.semesterRepository.GetAllAsync();

            return new TeacherDashboardReuqestDTO
            {
                CurrentTab = tab,
                Question = questions.ToList(),
                Topics = topics.ToList(),
                Semesters = semesters.Select(s => new SemesterResponseDTO
                {
                    SemesterID = s.SemesterID,
                    SemesterName = s.SemesterName
                }).ToList()
            };
        }
    }
}
