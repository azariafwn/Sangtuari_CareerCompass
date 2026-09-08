using System.ComponentModel.DataAnnotations;

namespace SangtuariCareerCompass.ViewModels
{
    public class CreatePsychologistViewModel
    {
        [Required(ErrorMessage = "Nama Lengkap wajib diisi.")]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email wajib diisi.")]
        [EmailAddress(ErrorMessage = "Format email tidak valid.")]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role wajib dipilih.")]
        public string Role { get; set; } = "Staff"; // "Head" atau "Staff"
    }
}