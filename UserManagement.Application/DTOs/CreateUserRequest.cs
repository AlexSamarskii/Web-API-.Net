using UserManagement.Domain.Enums;

namespace UserManagement.Application.DTOs
{
    public class CreateUserRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Gender Gender { get; set; } 
        public DateTime? Birthday { get; set; }
        public bool IsAdmin { get; set; }
        public string Requester { get; set; } = string.Empty; 
    }
}
