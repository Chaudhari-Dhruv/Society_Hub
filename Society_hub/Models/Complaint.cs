namespace Society_hub.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? ImagePath { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? ResolvedAt { get; set; }

        public int ResidentId { get; set; }
        public Resident? Resident { get; set; }

        public string? AssignedTo { get; set; }
    }
}
