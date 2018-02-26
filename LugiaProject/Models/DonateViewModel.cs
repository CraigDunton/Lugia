using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LugiaProject.Models
{
    public class DonateViewModel
    {
        public ApplicationUser User { get; set; }
        public List<NonProfitModel> NonProfitsList { get; set; }
    }
}
