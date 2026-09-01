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

        [MaxLength(50)]
        public string? Degree { get; set; }

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Role { get; set; } = "Staff"; // "Head" atau "Staff"

        [MaxLength(50)]
        public string? SilpNumber { get; set; }

        public DateOnly? SilpStartDate { get; set; }

        public DateOnly? SilpEndDate { get; set; }

        [MaxLength(50)]
        public string? StrNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}