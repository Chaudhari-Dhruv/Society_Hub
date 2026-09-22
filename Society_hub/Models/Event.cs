namespace Society_hub.Models
{
    public class Event
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public DateTime EventDate { get; set; }

        public int MaximumParticipants { get; set; }

        public ICollection<EventRegistration> Registrations { get; set; } =
            new List<EventRegistration>();
    }
}
