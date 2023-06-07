using MOTM.Models;

namespace MOTM.Models
{
    public class DbInitializer
    {
        public static void Initialize(MOTMContext context)
        {
            if(context.Services.Any())
            {
                return;
            }

            var services = new Service[]
            {
                new Service{ID=2,Name="Swedish Full Body",Description="A nordic massage",Price=420,Duration=100},
                new Service{ID=3,Name="Swedish Back, Neck & Shoulders",Description="Another nordic massage",Price=666,Duration=100},
                new Service{ID=5,Name="Deep Tissue Full Body",Description="Deep",Price=420,Duration=66},
                new Service{ID=7,Name="Deep Tissue Back, Neck & Shoulders",Description="Deeper",Price=666,Duration=66},
                new Service{ID=11,Name="Feet",Description="Yes, just feet",Price=69,Duration=42},
                new Service{ID=13,Name="Head",Description="Just the head",Price=69,Duration=42},
                new Service{ID=17,Name="Arms",Description="Arm massages are a thing",Price=69,Duration=42},
                new Service{ID=19,Name="Scrape Full Body",Description="Lots of Exfoliating",Price=420,Duration=100},
                new Service{ID=23,Name="Scrape Back",Description="Still plenty of exfoliating",Price=420,Duration=100},
                new Service{ID=29,Name="Express Facial",Description="Quick yet effective",Price=69,Duration=42},
                new Service{ID=31,Name="Facial",Description="Effective",Price=69,Duration=42},
                new Service{ID=37,Name="Pedicure",Description="Just the feet",Price=69,Duration=66},
                new Service{ID=41,Name="Manicure",Description="Just the hands",Price=69,Duration=66}
            };

            context.Services.AddRange(services);
            context.SaveChanges();
        }
    }
}