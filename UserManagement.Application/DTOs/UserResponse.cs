namespace UserManagement.Application.DTOs
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Gender { get; set; } 
        public DateTime? Birthday { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
    }
}
