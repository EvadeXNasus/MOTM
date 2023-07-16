using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOTM.Models
{
    [NotMapped]
    public class BookedSlot
    {
        public BookedSlot(DateTime BookedDay)
        {
            this.BookedDay = BookedDay.Date;
        }

        [Key]
        public DateTime BookedDay { get; set; }
    }
}