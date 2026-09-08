using System.ComponentModel.DataAnnotations;

namespace SangtuariCareerCompass.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Password Lama wajib diisi.")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password Baru wajib diisi.")]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "Password harus antara 8 - 12 karakter.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,12}$",
            ErrorMessage = "Password harus mengandung minimal 1 huruf besar, 1 huruf kecil, 1 angka, dan 1 karakter spesial.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Konfirmasi Password Baru wajib diisi.")]
        [Compare("NewPassword", ErrorMessage = "Konfirmasi password tidak cocok dengan password baru.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}