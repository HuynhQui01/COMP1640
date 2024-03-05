using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Comp1640.Models;

[Index("FeedbackId", Name = "IX_Contributions_FeedbackId")]
public partial class Contribution
{
    [Key]
    public int ConId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ConName { get; set; }

    public int? UserId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Status { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? File { get; set; }

    public int? FeedbackId { get; set; }

    [ForeignKey("FeedbackId")]
    [InverseProperty("Contributions")]
    public virtual Feedback? Feedback { get; set; }
}
