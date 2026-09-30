namespace Society_hub.Models
{
    public class ApartmentBlock
    {
        public int Id { get; set; }

        public string BlockName { get; set; } = string.Empty;

        public int TotalFlats { get; set; }

        public ICollection<Flat> Flats { get; set; } = new List<Flat>();
    }
}
