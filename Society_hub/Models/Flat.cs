namespace Society_hub.Models
{
    public class Flat
    {
        public int Id { get; set; }

        public string FlatNumber { get; set; } = string.Empty;

        public int FloorNumber { get; set; }

        public bool IsOccupied { get; set; }
        //foreign key
        public int ApartmentBlockId { get; set; }
        //navigation property
        public ApartmentBlock? ApartmentBlock { get; set; }

        public ICollection<Resident> Residents { get; set; } = new List<Resident>();
    }
}
