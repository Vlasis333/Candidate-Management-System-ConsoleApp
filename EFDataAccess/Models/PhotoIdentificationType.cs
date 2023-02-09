using System.ComponentModel.DataAnnotations;

namespace EFDataAccess.Models
{
    internal class PhotoIdentificationType
    {
        public int PhotoIdentificationTypeId { get; set; }
        [MaxLength(80)]
        public string Type { get; set; }

        public override string ToString()
        {
            if (Type != null)
            {
                return Type;
            }
            else
            {
                return "";
            }
        }
    }
}
