using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Comp1640.Models;

public class CustomUser : IdentityUser
{
    // public int FacId { get; set; }
    public string? FacName {get; set; }
    [InverseProperty("User")]
    public virtual ICollection<Contribution> Contributions { get; } = new List<Contribution>();

    // [ForeignKey("FacId")]
    // [InverseProperty("Users")]
    // public virtual Faculty Fac { get; set; } = null!;
}
