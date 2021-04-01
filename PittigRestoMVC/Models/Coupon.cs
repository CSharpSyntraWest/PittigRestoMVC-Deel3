using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PittigRestoMVC.Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Naam")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Type Korting")]
        public string CouponType { get; set; }
        public enum ECouponType { Percent = 0, Euro = 1 }
        [Required]
        [Display(Name = "Korting")]
        public double Discount { get; set; }

        [Required]
        [Display(Name = "Minimum bedrag")]
        public double MinimumAmount { get; set; }

        [Display(Name = "Afbeelding")]
        public byte[] Picture { get; set; }

        [Display(Name = "Is Actief")]
        public bool IsActive { get; set; }


    }
}
