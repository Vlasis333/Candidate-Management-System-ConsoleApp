using BetterConsoleTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EFDataAccess.Helpers;

namespace EFDataAccess.Models
{
    internal class Candidate
    {
        public int CandidateId { get; set; }
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string MiddleName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        [MaxLength(1)]
        public string Gender { get; set; }
        [MaxLength(100)]
        public string NativeLanguage { get; set; }
        public DateTime BirthDate { get; set; }

        // Connections to other entities
        public virtual CandidateLocation CandidateLocation { get; set; }
        public virtual CandidateContact CandidateContact { get; set; }
        public virtual CandidatePhotoIdentification CandidatePhotoIdentification { get; set; }
        // Connection to the Certificates entity
        public ICollection<CandidateCertificates> CandidateCertificates { get; set; }

        public override string ToString()
        {
            Table table = new Table("Candidate Numbe", "First Name", "Middle Name", "Last Name", "Gender", "Native Language", "Birth Date");
            table.AddRow(CandidateId, GlobalMethodsAndVariables.CheckIfStringIsNull(FirstName), GlobalMethodsAndVariables.CheckIfStringIsNull(MiddleName), 
                GlobalMethodsAndVariables.CheckIfStringIsNull(LastName), GlobalMethodsAndVariables.CheckIfStringIsNull(Gender), 
                GlobalMethodsAndVariables.CheckIfStringIsNull(NativeLanguage), BirthDate.ToShortDateString());

            return table.ToString();
        }
    }
}
