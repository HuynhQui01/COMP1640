using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Comp1640.Models;

[Table("User")]
public partial class User
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? UserName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Phone { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Address { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Password { get; set; }

    [Column("RoleID")]
    public int? RoleId { get; set; }

    [Column("FacID")]
    public int? FacId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Img { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Contribution> Contributions { get; } = new List<Contribution>();

    [ForeignKey("FacId")]
    [InverseProperty("Users")]
    public virtual Faculty? Fac { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role? Role { get; set; }
}
