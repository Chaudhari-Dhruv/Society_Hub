namespace Society_hub.Models
{
    public class SecurityGuard
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Shift { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        // Identity User Foreign Key
        public string? ApplicationUserId { get; set; }

        // Navigation Property
        public ApplicationUser? ApplicationUser { get; set; }
    }
}