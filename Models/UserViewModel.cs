using Microsoft.AspNetCore.Identity;
namespace Comp1640.Models
{
    public class UserRoleViewModel
    {
        public CustomUser User { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Faculties { get; set;}
    }
}

