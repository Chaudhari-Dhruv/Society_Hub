using System.ComponentModel.DataAnnotations;

namespace Society_hub.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [Display(Name = "Full Name")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a role.")]
        [Display(Name = "Account Role")]
        public string Role { get; set; } = "Resident"; // "Admin", "Resident", "SecurityGuard"

        // =========================
        // Resident-specific fields
        // =========================
        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string? Phone { get; set; }

        [Display(Name = "Flat Allocation")]
        public int? FlatId { get; set; }

        [Range(1, 50, ErrorMessage = "Family members must be at least 1.")]
        [Display(Name = "Number of Family Members")]
        public int FamilyMembers { get; set; } = 1;

        // =========================
        // Security Guard-specific fields
        // =========================
        [Display(Name = "Guard Phone Number")]
        public string? GuardPhone { get; set; }

        [Display(Name = "Guard Duty Shift")]
        public string? Shift { get; set; } = "Morning";
    }
}
