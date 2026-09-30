namespace Society_hub.Models
{
    public class EventRegistration
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Event? Event { get; set; }

        public int ResidentId { get; set; }
        public Resident? Resident { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}