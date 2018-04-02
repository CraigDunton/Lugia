using System.Linq;
using LugiaProject.Data;

namespace LugiaProject.Models
{
    public class SeedData
    {
        public static void Initialize(ApplicationDbContext dbContext)
        {

            if (!dbContext.NonProfits.Any())
            {
                dbContext.NonProfits.AddRange(

                    new NonProfitModel
                    {
                        Description = "Holla holla holla, feed dem boys",
                        Name = "Feed dem boys",
                        Points = 420,
                        PointsGoal = 12000
                    },

                    new NonProfitModel
                    {
                        Description = "Quench some thirst",
                        Name = "Water Guys",
                        Points = 8080,
                        PointsGoal = 15000
                    },

                    new NonProfitModel
                    {
                        Description = "Help stop the bad things",
                        Name = "Stop Bad",
                        Points = 9500,
                        PointsGoal = 20000
                    }

                );
            }

            if (!dbContext.Sponsors.Any())
            {
                dbContext.Sponsors.AddRange(
                    new SponsorModel
                    {
                        Name = "Nationwide",
                        Description = "Nationwide does cool things",
                        Points = 10000,
                        PointsGiven = 0
                    }
                );
            }

            dbContext.SaveChanges();
        }
    }
}
