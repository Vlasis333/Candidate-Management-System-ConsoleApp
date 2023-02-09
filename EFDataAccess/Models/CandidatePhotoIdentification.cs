using BetterConsoleTables;
using EFDataAccess.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFDataAccess.Models
{
    internal class CandidatePhotoIdentification
    {
        [ForeignKey("Candidate")]
        public int CandidatePhotoIdentificationId { get; set; }
        [MaxLength(40)]
        public string Number { get; set; }
        public DateTime IssueDate { get; set; }

        public virtual Candidate Candidate { get; set; }
        public virtual PhotoIdentificationType PhotoIdentificationType { get; set; }

        public override string ToString()
        {
            Table table = new Table("Photo Id Number", "Photo Id IssueDate", "Photo Id Type");
            table.AddRow(GlobalMethodsAndVariables.CheckIfStringIsNull(Number), IssueDate.ToShortDateString(), PhotoIdentificationType.ToString());

            return table.ToString();
        }
    }
}
