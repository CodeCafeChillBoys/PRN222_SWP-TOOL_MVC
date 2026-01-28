namespace PRN222_SWP_TOOL_MVC.Repository.Entities
{
    public class User
    {
        public int UserID { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }

        // NULL nếu login Google
        public string? PasswordHash { get; set; }

        public int RoleID { get; set; }

        // LOCAL | GOOGLE
        public string Provider { get; set; }
        public string? ProviderUserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Role Role { get; set; }
        public Student? Student { get; set; }
        public Teacher? Teacher { get; set; }
    }
}
