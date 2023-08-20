using System.ComponentModel.DataAnnotations;

namespace MOTM.Models
{
    public class Customer
    {
        [Key, StringLength(450)]
        public string Id { get; set; }
        [Required,StringLength(40), EmailAddress]
        public string EMail { get; set; }
        [Required,StringLength(30)]
        public string FirstName { get; set; }
        [Required,StringLength(30)]
        public string LastName { get; set; }
        [Required,Phone]
        public string PhoneNumber { get; set; }
        [Required,StringLength(30)]
        public string FirstAddressLine { get; set; }
        [StringLength(30)]
        public string? SecondAddressLine { get; set; }
        [StringLength(30)]
        public string? ThirdAddressLine { get; set; }
        [Required,StringLength(30)]
        public string PostTown { get; set; }
        [Required,StringLength(8)]
        public string PostCode { get; set; }
    }
}
