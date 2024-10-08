using Microsoft.AspNetCore.Identity;

namespace CustomerSupportAPI.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
