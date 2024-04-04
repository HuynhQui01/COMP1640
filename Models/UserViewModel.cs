using Microsoft.AspNetCore.Identity;
namespace Comp1640.Models
{
    public class UserRoleViewModel
    {
        public CustomUser User { get; set; }
        public List<string> Roles { get; set; }
        public int Faculties { get; set;}
    }
}

