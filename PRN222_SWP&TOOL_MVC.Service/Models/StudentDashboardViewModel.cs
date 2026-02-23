using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222_SWP_TOOL_MVC.Repository.Entities;

namespace PRN222_SWP_TOOL_MVC.Service.Models
{
    public class StudentDashboardViewModel
    {
            public string CurrentTab { get; set; }

            public List<StudentGroup> Groups { get; set; }
            public List<Topic> Topics { get; set; }
            public List<Question> Questions { get; set; }
    }
}
