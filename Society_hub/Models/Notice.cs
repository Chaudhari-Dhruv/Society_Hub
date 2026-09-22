namespace Society_hub.Models
{
    public class Notice
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime PublishedDate { get; set; } = DateTime.Now;

        public DateTime? ExpiryDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
