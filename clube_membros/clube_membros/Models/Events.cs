using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace clube_membros.Models
{
    public class Events
    {
        
        public string Id { get; set; }

        //[Required]
        [StringLength(50)]
        public string Name { get; set; }


        public string City { get; set; }

        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; }

        //[RegularExpression(@"\d{3,9})] a special character and password between 3 to 9 characters
    }
}