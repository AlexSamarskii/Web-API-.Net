namespace UserManagement.Application.DTOs
{
    public class UpdateUserRequest
    {
        public string Login { get; set; } = string.Empty;
        public string? Name { get; set; }
        public DateTime? Birthday { get; set; }
        public string Requester { get; set; } = string.Empty;
    }
}
