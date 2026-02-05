using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_SWP_TOOL_MVC.Repository.Entities
{
    public class StudentGroup
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public int ClassID { get; set; }
        public int CreatedByUserID { get; set; } 
        public int MaxMember {  get; set; }
        public string InviteCode { get; set; }
         public bool IsLocked { get; set; } = false;
        public Class Class { get; set; }


        public string Status { get; set; }
       

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Optional: lưu người tạo (UserID)
      
        public User CreatedByUser { get; set; }

        public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
    }
}
