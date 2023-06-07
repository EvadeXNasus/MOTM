using System.ComponentModel.DataAnnotations.Schema;

namespace MOTM.Models
{
    [NotMapped]
    public class CustomerOrder
    {
        public string CustomerId { get; set; }
        public string EMail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstAddressLine { get; set; }
        public string? SecondAddressLine { get; set; }
        public string? ThirdAddressLine { get; set; }
        public string PostTown { get; set; }
        public string PostCode { get; set; }
        public long Services { get; set; }
        public DateTime OrderedTime { get; set; }
        public DateTime TimeSlot { get; set; }
        public bool Fulfilled { get; set; }
    }
}
