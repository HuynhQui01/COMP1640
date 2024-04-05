using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Comp1640.Models;

public class CustomUser : IdentityUser
{
    
    public int? FacId { get; set; }
    [InverseProperty("User")]
    public virtual ICollection<Contribution> Contributions { get; } = new List<Contribution>();

    [InverseProperty("User")]
    public virtual ICollection<Feedback> Feedbacks { get; } = new List<Feedback>();

    public string? ProfileImagePath { get; set; }
    public string FullName { get; set; }

    [ForeignKey("FacId")]
    [InverseProperty("CustomUser")]
    public virtual Faculty? Faculty { get; set; }

}

