using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TastyTreats.Model.DTO
{
    public class RecipeReviewsDTO
    {
        public RecipeDTO Details { get; set; }
        public List<ReviewDTO> Reviews { get; set; }
    }
}
