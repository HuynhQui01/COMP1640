using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Comp1640.Models;

[Index("Ayid", Name = "IX_Faculties_AYId")]
public partial class Faculty
{
    [Key]
    [Column("FacID")]
    public int FacId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? FacName { get; set; }

    [Column("AYId")]
    public int? Ayid { get; set; }

    [ForeignKey("Ayid")]
    [InverseProperty("Faculties")]
    public virtual AcademicYear? Ay { get; set; }

    [InverseProperty("Fac")]
    public virtual ICollection<Contribution> Contributions { get; } = new List<Contribution>();

    // [InverseProperty("Fac")]
    // public virtual ICollection<CustomUser> Users { get; } = new List<CustomUser>();
}
