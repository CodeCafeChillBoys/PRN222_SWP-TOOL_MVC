using System.ComponentModel.DataAnnotations;

namespace PRN222_SWP_TOOL_MVC.Service.DTO.Request
{
    public class CreateTopicRequestDTO
    {
    
        public string TopicName { get; set; }
        public string? Description { get; set; }
      
        public string? Requirement { get; set; }
     
        public int SemesterID { get; set; }
        public int? MaxGroupCount { get; set; }
    }
}
