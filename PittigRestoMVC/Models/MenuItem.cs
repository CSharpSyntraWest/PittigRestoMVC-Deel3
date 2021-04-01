using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PittigRestoMVC.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Naam")]
        public string Name { get; set; }
        [Display(Name = "Beschrijving")]
        public string Description { get; set; }
        [Display(Name = "Pittigheid")]
        public string Spicyness { get; set; }
        public enum ESpicy { NA = 0, Geen = 1, Weinig = 2, Sterk = 3 }

        [Display(Name = "Foto")]
        public string Image { get; set; }

        [Display(Name = "Categorie")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Display(Name = "Sub categorie")]
        public int SubCategoryId { get; set; }

        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = " Prijs moet hoger zijn dan €{1}")]
        [Display(Name = "Prijs (EUR)")]
        public double Price { get; set; }

    }
}
