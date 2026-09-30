namespace Society_hub.Models
{
    public class Visitor
    {
        public int Id { get; set; }

        public string VisitorName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Purpose { get; set; } = string.Empty;

        public DateTime VisitDate { get; set; }

        public DateTime? EntryTime { get; set; }

        public DateTime? ExitTime { get; set; }

        public string Status { get; set; } = "Expected";

        // Foreign Key
        public int ResidentId { get; set; }

        // Navigation Property
        public Resident? Resident { get; set; }
    }
}