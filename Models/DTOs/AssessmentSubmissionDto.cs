using System;
using System.ComponentModel.DataAnnotations;

namespace SangtuariCareerCompass.Models.DTOs
{
    public class AssessmentSubmissionDto
    {
        [Required]
        public string AssessmentType { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string SchoolName { get; set; }
        [Required]
        public string ClassName { get; set; }
        [Required]
        public string Major { get; set; }

        // Data dari Page 3 dimasukkan ke sini
        public string FatherJob { get; set; }
        public string MotherJob { get; set; }
        public string Hobby { get; set; }
        public string Goals { get; set; }
        public string[] LikedSubjects { get; set; }
        public string[] DislikedSubjects { get; set; }
    }
}