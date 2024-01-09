using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamestore.models
{
    public class Product
    {
        private const int v = 10000;

        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(1, v)]
        [Display(Name = "List Price")]

        public double ListPrice { get; set; }
        [Required]
        [Range(1, v)]
        public double Price { get; set; }
        [Required]
        [Range(1, v)]
        [Display(Name = "Price 5-10")]

        public double Price5 { get; set; }
        [Required]
        [Range(1, v)]
        [Display(Name = "Price 10+")]

        public double Price10 { get; set; }
        [ValidateNever]

        public string ImageUrl { get; set; }
        [Required]
        [Display(Name = "Category")]

        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        [ValidateNever]
        public Category Category { get; set; }
        [Required]
        [Display(Name ="Cover Type")]
        public int CoverTypeId  { get; set; }
        [ForeignKey(nameof(CoverTypeId))]
        [ValidateNever]

        public CoverType CoverType { get; set; }

    }
}
