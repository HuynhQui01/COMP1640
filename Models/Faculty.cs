using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Comp1640.Models;

public partial class Faculty
{
    [Key]
    public int FacId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string FacName { get; set; } = null!;

    [Column("AYId")]
    public int? Ayid { get; set; }

    [ForeignKey("Ayid")]
    [InverseProperty("Faculties")]
    public virtual AcademicYear? Ay { get; set; }

    [InverseProperty("Fac")]
    public virtual ICollection<Contribution> Contributions { get; } = new List<Contribution>();
}
