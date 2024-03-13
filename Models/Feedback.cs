using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Comp1640.Models;

public partial class Feedback
{
    [Key]
    public int FeedbackId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Comment { get; set; }

    [Column("FBTime", TypeName = "datetime")]
    public DateTime? Fbtime { get; set; }

    [StringLength(450)]
    public string? UserId { get; set; }

    [InverseProperty("Feedback")]
    public virtual ICollection<Contribution> Contributions { get; } = new List<Contribution>();

    [ForeignKey("UserId")]
    [InverseProperty("Feedbacks")]
    public virtual AspNetUser? User { get; set; }
}
