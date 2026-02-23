using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_SWP_TOOL_MVC.Repository.Entities
{
    public class GroupMember
    {
        public int GroupID { get; set; }
        public StudentGroup Group { get; set; }

        // StudentID của bạn đang là FK tới User (StudentID (FK to User))
        public int StudentID { get; set; }
        public Student Student { get; set; }

        public bool IsLeader { get; set; } = false;

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
