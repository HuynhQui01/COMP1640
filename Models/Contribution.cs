using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Comp1640.Models;

[Index("FacId", Name = "IX_Contributions_FacId")]
[Index("FeedbackId", Name = "IX_Contributions_FeedbackId")]
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
    
    [StringLength(450)]
    public string? ImageFilePath { get; set; }

    public int? FeedbackId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime SubmitDate { get; set; }

    public int? FacId { get; set; }

    [ForeignKey("FacId")]
    [InverseProperty("Contributions")]
    public virtual Faculty? Fac { get; set; }

    [InverseProperty("Con")]
    public virtual ICollection<Feedback> Feedbacks { get; } = new List<Feedback>();

    [ForeignKey("UserId")]
    [InverseProperty("Contributions")]
    public virtual CustomUser User { get; set; } = null!;
}
