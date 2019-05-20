using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace Clube_de_Membros.Models
{
    public class Members
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        public string Image { get; set; }
    }
}