using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PittigRestoMVC.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Naam van sub-category")]
        [Required(ErrorMessage = "Naam is verplicht")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }
    }
}
