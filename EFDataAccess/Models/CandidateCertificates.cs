using System;
using System.Collections.Generic;

namespace EFDataAccess.Models
{
    internal class CandidateCertificates 
    {
        public int CandidateCertificatesId { get; set; }
        public DateTime ExaminationDate { get; set; }

        // Connections to other entities
        public virtual Certificate Certificate { get; set; }
        public virtual CertificateAssessment CertificateAssessment { get; set; } 
        public ICollection<TopicAssesment> TopicAssesments { get; set; }

        public override string ToString()
        {
            return ExaminationDate.ToShortDateString();
        }
    }
}
