﻿using System;

namespace EFDataAccess.Models
{
    internal class TopicAssesment
    {
        public int TopicAssesmentId { get; set; }
        public Int16 AwardedMarks { get; set; }

        public virtual CandidateCertificates CandidateCertificates { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
