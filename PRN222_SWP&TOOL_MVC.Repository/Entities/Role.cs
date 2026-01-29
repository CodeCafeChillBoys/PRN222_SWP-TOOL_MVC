namespace PRN222_SWP_TOOL_MVC.Repository.Entities
{
    public class Role
    {
        public int RoleID { get; set; }

        public string RoleCode { get; set; }   // STUDENT, TEACHER, ADMIN
        public string RoleName { get; set; }   // Học sinh, Giảng viên, Quản trị
        public string? Description { get; set; }

        // Navigation
        public ICollection<User> Users { get; set; }
    }
}
