using System;
using System.ComponentModel.DataAnnotations;

namespace SangtuariCareerCompass.ViewModels
{
    public class PsychologistProfileViewModel
    {
        [Required(ErrorMessage = "Nama Lengkap wajib diisi.")]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty; // Hanya untuk ditampilkan

        [MaxLength(50)]
        public string? Degree { get; set; }

        [MaxLength(50)]
        public string? SilpNumber { get; set; }

        public DateOnly? SilpStartDate { get; set; }

        public DateOnly? SilpEndDate { get; set; }

        [MaxLength(50)]
        public string? StrNumber { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SilpStartDate.HasValue && SilpEndDate.HasValue)
            {
                if (SilpEndDate.Value < SilpStartDate.Value)
                {
                    yield return new ValidationResult(
                        "Tanggal Berakhir SILP tidak boleh lebih awal dari Tanggal Mulai SILP.",
                        new[] { nameof(SilpEndDate) } // Pesan error akan muncul spesifik di input ini
                    );
                }
            }
        }
    }
}