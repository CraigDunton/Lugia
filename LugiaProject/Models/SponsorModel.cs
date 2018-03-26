using System;
using System.ComponentModel.DataAnnotations;

namespace LugiaProject.Models
{
    public class SponsorModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public int PointsGiven { get; set; }
    }
}
