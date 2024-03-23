using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comp1640.Models;

namespace Comp1640.Models
{
    public class ContributionFeedbackView
    {
        public Contribution contribution { get; set; }
        public Feedback feedback { get; set; }
        public List<Feedback> feedbacks { get; set; }
    }
}