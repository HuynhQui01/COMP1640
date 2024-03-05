using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Comp1640.Models;

public partial class Contribution
{
    [Key]
    [Column("ConID")]
    public int ConId { get; set; }

    [StringLength(50)]
    public string ConName { get; set; } = null!;

    [Column("UserID")]
    [StringLength(450)]
    public string UserId { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Filepath { get; set; }

    public int FeedbackId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime SubmitDate { get; set; }

    [ForeignKey("FeedbackId")]
    [InverseProperty("Contributions")]
    public virtual Feedback Feedback { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Contributions")]
    public virtual AspNetUser User { get; set; } = null!;
}
