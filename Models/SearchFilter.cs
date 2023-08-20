using System.ComponentModel.DataAnnotations;

namespace MOTM.Models
{
    public class SearchFilter
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime OrderedMonth { get; set; }
        public DateTime BookedMonth { get; set; }
    }
}
