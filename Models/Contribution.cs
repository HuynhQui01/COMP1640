using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Comp1640.Models;
[Index("UserId", Name = "IX_Contributions_UserID")]
public partial class Contribution
{
    [Key]
    [Column("ConID")]
    public int ConId { get; set; }

    [StringLength(50)]
    public string ConName { get; set; } = null!;

    [Column("UserID")]
    public string UserId { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [StringLength(450)]
    public string? Filepath { get; set; }

    public string? ImageFilePath { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime SubmitDate { get; set; }

    public string? Buplic { get; set; }

    [Column("AYId")]
    public int? Ayid { get; set; }

    [ForeignKey("Ayid")]
    [InverseProperty("Contributions")]
    public virtual AcademicYear? Ay { get; set; }

    [InverseProperty("Con")]
    public virtual ICollection<Feedback> Feedbacks { get; } = new List<Feedback>();

    [ForeignKey("UserId")]
    [InverseProperty("Contributions")]
    public virtual CustomUser User { get; set; } = null!;
}
