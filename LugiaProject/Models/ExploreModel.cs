using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LugiaProject.Models
{
    public class ExploreModel
    {
        public ExploreModel()
        {
            Query = "";
            Result = new List<StumbleModel>();
        }

        [Required]
        public string Query { get; set; }

        [Required]
        public List<StumbleModel> Result {get; set;}
    }
}
