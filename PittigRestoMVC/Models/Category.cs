using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PittigRestoMVC.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Display(Name="Naam van Category")]
        [Required(ErrorMessage ="Naam is verplicht")]
        public string Name { get; set; }
    }
}
