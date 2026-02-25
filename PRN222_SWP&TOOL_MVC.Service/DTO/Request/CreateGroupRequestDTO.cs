using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_SWP_TOOL_MVC.Service.DTO.Request
{
    public class CreateGroupRequestDTO
    {
        public int ClassID { get; set; }
        
        public string GroupName { get; set; }

        /// <summary>Số thành viên tối đa, mặc định 5 nếu không truyền</summary>
        public int MaxMember { get; set; } = 5;
    }
}
