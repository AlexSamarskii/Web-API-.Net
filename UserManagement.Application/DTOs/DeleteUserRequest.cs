namespace UserManagement.Application.DTOs
{
    public class DeleteUserRequest
    {
        public string Login { get; set; } = string.Empty;
        public bool SoftDelete { get; set; } = true;
        public string Requester { get; set; } = string.Empty;
    }
}
