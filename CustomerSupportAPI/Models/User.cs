using Microsoft.AspNetCore.Identity;

namespace CustomerSupportAPI.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        //public bool IsApproved { get; set; } = true; //New property to check user is approved or not
    }
}
