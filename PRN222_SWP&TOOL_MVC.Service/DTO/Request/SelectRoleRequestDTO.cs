namespace PRN222_SWP_TOOL_MVC.Service.DTO.Request
{
    public class SelectRoleRequestDTO
    {
        public string email { get; set; }
        public string name { get; set; }

        public int  roleId { get; set; }
        public string ProviderUserId { get; set; }
    }
}
