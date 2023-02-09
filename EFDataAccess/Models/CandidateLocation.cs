using BetterConsoleTables;
using EFDataAccess.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFDataAccess.Models
{
    internal class CandidateLocation
    {
        [ForeignKey("Candidate")]
        public int CandidateLocationId { get; set; }
        [MaxLength(120)]
        public string Address { get; set; }
        [MaxLength(120)]
        public string Address2 { get; set; }
        [MaxLength(120)]
        public string Residence { get; set; }
        [MaxLength(120)]
        public string Province { get; set; }
        [MaxLength(100)]
        public string City { get; set; }
        [MaxLength(15)]
        public string PostalCode { get; set; }

        public virtual Candidate Candidate { get; set; }

        public override string ToString()
        {
            Table table = new Table("Address", "Address2", "Residence", "Province", "City", "PostalCode");
            table.AddRow(GlobalMethodsAndVariables.CheckIfStringIsNull(Address), GlobalMethodsAndVariables.CheckIfStringIsNull(Address2), 
                GlobalMethodsAndVariables.CheckIfStringIsNull(Residence), GlobalMethodsAndVariables.CheckIfStringIsNull(Province), 
                GlobalMethodsAndVariables.CheckIfStringIsNull(City), GlobalMethodsAndVariables.CheckIfStringIsNull(PostalCode));

            return table.ToString();
        }
    }
}
