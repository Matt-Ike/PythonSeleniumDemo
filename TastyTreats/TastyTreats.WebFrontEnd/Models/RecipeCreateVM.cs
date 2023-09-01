using Microsoft.AspNetCore.Mvc.Rendering;
using TastyTreats.Model.Entities;

namespace TastyTreats.WebFrontEnd.Models
{
    public class RecipeCreateVM : Recipe
    {
        public IEnumerable<SelectListItem> RecipeTypes { get; set; }

        public IEnumerable<SelectListItem> Chefs { get; set; }
    }
}
