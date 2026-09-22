namespace Society_hub.Models
{

        public class MaintenanceBill
        {
            public int Id { get; set; }

            public string BillNumber { get; set; } = string.Empty;

            public decimal Amount { get; set; }

            public DateTime BillDate { get; set; }

            public DateTime DueDate { get; set; }

            public string Status { get; set; } = "Pending";

            public int ResidentId { get; set; }
            public Resident? Resident { get; set; }

            public ICollection<Payment> Payments { get; set; } =
                new List<Payment>();
        }
 }

