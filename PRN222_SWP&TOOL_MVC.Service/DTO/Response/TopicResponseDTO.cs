namespace PRN222_SWP_TOOL_MVC.Service.DTO.Response
{
    public class TopicResponseDTO
    {
        public int TopicID { get; set; }

        public string TopicName { get; set; }

        public string? Description { get; set; }

        public string? Requirement { get; set; }

        public int SemesterID { get; set; }

        public int TeacherID { get; set; }

        public int? MaxGroupCount { get; set; }

        public DateTime CreatedAt { get; set; }
    }

}
