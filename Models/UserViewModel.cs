using Microsoft.AspNetCore.Identity;
namespace Comp1640.Models
{
    public class UserRoleViewModel
    {
        public IdentityUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}

