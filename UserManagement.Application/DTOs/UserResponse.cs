using UserManagement.Domain.Enums;

public class UserResponse
{
    public string Name { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public bool IsActive { get; set; }
}