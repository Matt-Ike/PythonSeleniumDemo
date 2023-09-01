using Microsoft.AspNetCore.Mvc.Rendering;
using TastyTreats.Model.Entities;

namespace TastyTreats.WebFrontEnd.Models
{
    public class RecipeEditVM
    {
        public Recipe Recipe { get; set; }

        public IEnumerable<SelectListItem> RecipeTypes { get; set; }

        public IEnumerable<SelectListItem> Chefs { get; set; }
    }
}
