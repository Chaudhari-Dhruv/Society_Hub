namespace Society_hub.Models
{
    public class MaintenanceBill
    {
        public int Id { get; set; }

        public int ResidentId { get; set; }
        public Resident? Resident { get; set; }

        public decimal Amount { get; set; }

        public DateTime BillingMonth { get; set; }

        public DateTime DueDate { get; set; }

        public string PaymentStatus { get; set; } = "Pending";

        public DateTime? PaymentDate { get; set; }

        public string? ReceiptNumber { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}