using Microsoft.AspNetCore.Identity;
namespace Society_hub.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
