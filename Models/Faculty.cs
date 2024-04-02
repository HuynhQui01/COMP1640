using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Comp1640.Models;


public partial class Faculty
{
    [Key]
    [Column("FacID")]
    public int FacId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? FacName { get; set; }

    [InverseProperty("Faculty")]
    public virtual ICollection<CustomUser> CustomUser { get; } = new List<CustomUser>();
}
