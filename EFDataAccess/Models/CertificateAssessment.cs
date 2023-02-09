using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFDataAccess.Models
{
    internal class CertificateAssessment
    {
        [ForeignKey("CandidateCertificates")]
        public int CertificateAssessmentId { get; set; }
        public DateTime ScoreReportDate { get; set; }
        public Int16 CandidateScore { get; set; }
        public Int16 MaximumScore { get; set; }
        public Int16 PercentageScore { get; set; }
        [MaxLength(40)]
        public string AssessmentResult { get; set; }

        public virtual CandidateCertificates CandidateCertificates { get; set; }
    }
}
