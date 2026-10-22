namespace SangtuariCareerCompass.Models.DTOs
{
    public class WorkloadAllocationDto
    {
        public string SchoolName { get; set; }
        // Key: PsychologistId, Value: Jumlah peserta yang diassign
        public Dictionary<Guid, int> Allocations { get; set; }
    }
}
