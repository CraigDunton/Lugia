﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LugiaProject.Models
{
    public class NonProfitModel
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public int Points { get; set; }
        public int PointsGoal { get; set; }
        public int PointsFromUser { get; set; }

        public NonProfitModel()
        {
            PointsFromUser = 0;
        }
    }
}
