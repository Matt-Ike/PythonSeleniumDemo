using Microsoft.AspNetCore.Mvc.Rendering;
using TastyTreats.Model.DTO;

namespace TastyTreats.WebFrontEnd.Models
{
    public class RecipeDetailsVM
    {
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string Chef { get; set; }
        public int Yield { get; set; }
        public bool Archived { get; set; }
        public IEnumerable<ReviewDTO> Reviews { get; set; }
    }
}
