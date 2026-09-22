namespace Society_hub.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public string TransactionId { get; set; } = string.Empty;

        public int MaintenanceBillId { get; set; }
        public MaintenanceBill? MaintenanceBill { get; set; }
    }
}
