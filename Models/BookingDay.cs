namespace MOTM.Models
{
    public class BookingDay
    {
        public DateTime[] TimeSlots { get; set; } = new DateTime[4];
        public bool[] SlotsAreBooked { get; set; } = new bool[4];
        public bool Overbooked()
        {
            byte BookedCount = 0;
            foreach (bool Slot in SlotsAreBooked)
            {
                if (Slot)
                {
                    BookedCount++;
                }
            }
            return BookedCount > 3;
        }
    }
}