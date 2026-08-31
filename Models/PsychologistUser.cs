using System;
using System.ComponentModel.DataAnnotations;

namespace SangtuariCareerCompass.Models
{
    public class PsychologistUser
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Role { get; set; } = "Staff"; // "Head" atau "Staff"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}