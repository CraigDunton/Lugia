using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LugiaProject.Models
{
    public class AInterest
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public bool IsInterest { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
