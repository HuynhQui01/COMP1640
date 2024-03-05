using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Comp1640.Models;

[Table("AcademicYear")]
public partial class AcademicYear
{
    [Key]
    [Column("AYId")]
    public int Ayid { get; set; }

    [Column(TypeName = "date")]
    public DateTime? CloseDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime? FinalCloseDate { get; set; }

    [InverseProperty("Ay")]
    public virtual ICollection<Faculty> Faculties { get; } = new List<Faculty>();
}
