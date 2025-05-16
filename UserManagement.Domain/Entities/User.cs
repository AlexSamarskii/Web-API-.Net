using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Login must contain only Latin letters and digits")]
        public string Login { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Password must contain only Latin letters and digits")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[a-zA-Zа-яА-Я]+$", ErrorMessage = "Name must contain only Latin or Cyrillic letters")]
        public string Name { get; set; } = string.Empty;

        public int Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public bool IsAdmin { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? ModifiedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? RevokedOn { get; set; }

        public string? RevokedBy { get; set; }
    }
}
