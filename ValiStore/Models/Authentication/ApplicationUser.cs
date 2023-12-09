using Microsoft.AspNetCore.Identity;

namespace ValiStore.Models.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string Roles { get; set; }
    }
}
