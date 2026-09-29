namespace Society_hub.Models
{
    public class Event
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public DateTime EventDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<EventRegistration> EventRegistrations { get; set; }
            = new List<EventRegistration>();
    }
}