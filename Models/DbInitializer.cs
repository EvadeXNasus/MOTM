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

            Service[] ActiveServices = new Service[]
            {
                new Service{ID=2,SortID=1,Name="Swedish Full Body",Description="A relaxing traditional Swedish massage covering the whole body",Price=160,Duration=60,Active=true},
                new Service{ID=3,SortID=2,Name="Swedish Back, Neck & Shoulders",Description="A relaxing traditional Swedish massage targeting key areas",Price=80,Duration=30,Active=true},
                new Service{ID=5,SortID=3,Name="Deep Tissue Full Body",Description="A modified Swedish massage covering the whole body",Price=200,Duration=60,Active=true},
                new Service{ID=7,SortID=4,Name="Deep Tissue Back, Neck & Shoulders",Description="A modified Swedish massage targeting key areas",Price=100,Duration=30,Active=true},
                new Service{ID=11,SortID=5,Name="Feet",Description="A podiatrist-grade foot treatment",Price=40,Duration=30,Active=true},
                new Service{ID=13,SortID=6,Name="Head",Description="A procedure which aims to relieve migraines and other capital issues",Price=30,Duration=30,Active=true},
                new Service{ID=17,SortID=7,Name="Arms",Description="A session which reinvigorates your busiest bodyparts",Price=35,Duration=30,Active=true},
                new Service{ID=19,SortID=8,Name="Reflexology",Description="Pushing the right buttons to restore balance to the body",Price=70,Duration=30,Active=true},
                new Service{ID=23,SortID=9,Name="Vacuum",Description="Negative pressure relieves problems caused by circulatory issues",Price=80,Duration=40,Active=true},
                new Service{ID=29,SortID=10,Name="Express Facial",Description="Fast-paced facial massage consisting of the essentials",Price=30,Duration=30,Active=true},
                new Service{ID=31,SortID=11,Name="Full Facial",Description="A thorough procedure fully rehabilitating the face's capabilities",Price=60,Duration=60,Active=true}
            };

            Service[] InactiveServices = new Service[]
            {
                new Service{ID=37,SortID=12,Name="RESERVED",Description="RESERVED FOR FUTURE USE",Price=0,Duration=0,Active=false},
                new Service{ID=41,SortID=13,Name="RESERVED",Description="RESERVED FOR FUTURE USE",Price=0,Duration=0,Active=false},
                new Service{ID=43,SortID=14,Name="RESERVED",Description="RESERVED FOR FUTURE USE",Price=0,Duration=0,Active=false},
                new Service{ID=47,SortID=15,Name="RESERVED",Description="RESERVED FOR FUTURE USE",Price=0,Duration=0,Active=false},
                new Service{ID=53,SortID=16,Name="RESERVED",Description="RESERVED FOR FUTURE USE",Price=0,Duration=0,Active=false},
                new Service{ID=59,SortID=17,Name="RESERVED",Description="RESERVED FOR FUTURE USE",Price=0,Duration=0,Active=false},
                new Service{ID=61,SortID=18,Name="RESERVED",Description="RESERVED FOR FUTURE USE",Price=0,Duration=0,Active=false},
                new Service{ID=67,SortID=19,Name="RESERVED",Description="RESERVED FOR FUTURE USE",Price=0,Duration=0,Active=false},
                new Service{ID=71,SortID=20,Name="RESERVED",Description="RESERVED FOR FUTURE USE",Price=0,Duration=0,Active=false},
            };

            context.Services.AddRange(ActiveServices);
            context.Services.AddRange(InactiveServices);
            context.SaveChanges();
        }
    }
}