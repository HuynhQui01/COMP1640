﻿using System;
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

    public string Name { get; set; } = null!;

    [InverseProperty("Ay")]
    public virtual ICollection<Contribution> Contributions { get; } = new List<Contribution>();

}
