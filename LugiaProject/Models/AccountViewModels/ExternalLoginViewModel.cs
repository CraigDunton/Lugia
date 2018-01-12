using LugiaProject.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LugiaProject.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        public ExternalLoginViewModel()
        {
            Interests = new List<AInterest>();
            foreach (var interest in InterestConstants.InterestList)
            {
                var i = new AInterest()
                {
                    Name = interest,
                    IsInterest = false
                };
                Interests.Add(i);
            }
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public List<AInterest> Interests { get; set; }
    }
}
