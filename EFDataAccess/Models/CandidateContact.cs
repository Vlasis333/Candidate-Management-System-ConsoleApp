using BetterConsoleTables;
using EFDataAccess.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFDataAccess.Models
{
    internal class CandidateContact
    {
        [ForeignKey("Candidate")]
        public int CandidateContactId { get; set; }
        [MaxLength(120)]
        public string LandlineNumber { get; set; }
        [MaxLength(120)]
        public string MobileNumber { get; set; }
        [MaxLength(255)]
        public string Email { get; set; }

        public virtual Candidate Candidate { get; set; }

        public override string ToString()
        {
            Table table = new Table("Land Line Number", "Mobile Number", "EMail");
            table.AddRow(GlobalMethodsAndVariables.CheckIfStringIsNull(LandlineNumber), GlobalMethodsAndVariables.CheckIfStringIsNull(MobileNumber), 
                GlobalMethodsAndVariables.CheckIfStringIsNull(Email));

            return table.ToString();
        }
    }
}
