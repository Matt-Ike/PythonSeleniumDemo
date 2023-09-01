using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TastyTreats.Model.Entities
{
    public class Recipe : BaseEntity
    {
        [Display(Name = "ID")]
        public int RecipeId { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Please enter the title.")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 character in length")]
        public string Title { get; set; }

        [Display(Name = "Chef")]
        [Required(ErrorMessage = "Please select a chef.")]
        public int ChefId { get; set; }

        [Display(Name = "Yield")]
        [Required(ErrorMessage = "Please state the yield of the recipe.")]
        [Range(2, 12, ErrorMessage = "The yield can only be between 2 and 12 inclusive")]
        public int Yield { get; set; }

        [Display(Name = "Date Added")]
        [Required(ErrorMessage = "Please provide a date.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateAdded { get; set; }

        [Display(Name = "Archived")]
        [Required(ErrorMessage = "Please indicate if the recipe is archived.")]
        public bool Archived { get; set; }

        [Display(Name = "Recipe Type")]
        [Required(ErrorMessage = "Please select a recipe type.")]
        public int RecipeTypeId { get; set; } 
    }
}
