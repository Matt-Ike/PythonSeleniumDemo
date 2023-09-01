using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TastyTreats.Model.DTO
{
    public class RecipeDTO
    {
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public int ChefId { get; set; }
        public int Yield { get; set; }

        [Display(Name = "Date Added")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateAdded { get; set; }
        public bool Archived { get; set; }
        public int RecipeTypeId { get; set; }
        public string ChefFirstName { get; set; }
        public string ChefLastName { get; set; }
        
        [Display(Name = "Chef")]
        public string ChefFullName
        {
            get { return $"{ChefLastName}, {ChefFirstName}"; }
        }
        public int ChefTypeId { get; set; }
        public int RecipeType { get; set; }

        [Display(Name = "Type")]
        public string Name { get; set; }
    }
}
