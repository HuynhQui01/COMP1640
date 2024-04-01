using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Comp1640.Models;

[Index("ConId", Name = "IX_Feedbacks_ConId")]
[Index("UserId", Name = "IX_Feedbacks_UserId")]
public partial class Feedback
{
    [Key]
    public int FeedbackId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Comment { get; set; }

    [Column("FBTime", TypeName = "datetime")]
    public DateTime? Fbtime { get; set; }

    public string? UserId { get; set; }

    public int? ConId { get; set; }

    [ForeignKey("ConId")]
    [InverseProperty("Feedbacks")]
    public virtual Contribution? Con { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Feedbacks")]
    public virtual CustomUser? User { get; set; }
}
