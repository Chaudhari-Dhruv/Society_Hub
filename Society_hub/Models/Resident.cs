namespace Society_hub.Models
{
    public class Resident
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string? ProfilePhoto { get; set; }

        public int FamilyMembers { get; set; }

        public int FlatId { get; set; }
        public Flat? Flat { get; set; }

        public ICollection<Visitor> Visitors { get; set; } = new List<Visitor>();

        public ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

        public ICollection<EventRegistration> EventRegistrations { get; set; } =
            new List<EventRegistration>();
    }
}
