using Microsoft.AspNetCore.Mvc;
using TastyTreats.Model.DTO;
using TastyTreats.Model.Entities;
using TastyTreats.Service;

namespace TastyTreats.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class RecipesController : Controller
    {
        private readonly RecipeService service = new();

        #region Asynchronous

        //GET: api/recipes
        public async Task<ActionResult<List<RecipeDTO>>> Get()
        {
            try
            {
                List<RecipeDTO> recipes = await service.GetAllWithDetailsAsync();
                return recipes;
            }
            catch (Exception ex) 
            {
                return Problem(title: "An internal error has occured. Please contact the system admin.");
            }
        }


        //GET: api/recipe/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Recipe>> Get(int id)
        {
            try
            {
                Recipe? recipe = await service.GetAsync(id);

                if (recipe == null)
                    return NotFound();

                return recipe;
            }
            catch (Exception ex)
            {
                return Problem(title: "An internal error has occurred. Please contact the system admin.");
            }
        }


        //GET: api/recipes/withreviews/{id}
        [HttpGet("withreviews/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RecipeReviewsDTO>> GetRecipeWithReviews(int id)
        {
            try
            {
                RecipeReviewsDTO? recipeWithReviews = await service.GetRecipeWithReviewsAsync(id);

                if (recipeWithReviews == null)
                    return NotFound();

                return recipeWithReviews;
            }
            catch (Exception ex)
            {
                return Problem(title: "An internal error has occurred. Please contact the system admin.");
            }
        }


        //PUT: api/recipes/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Recipe>> UpdateRecipe(int id, Recipe recipe)
        {
            try
            {
                if(id != recipe.RecipeId)
                    return BadRequest();

                recipe = await service.UpdateRecipeAsync(recipe);

                if (recipe.Errors.Count > 0)
                    return BadRequest(recipe.Errors);

                return recipe;
            }
            catch (Exception ex)
            {
                return Problem(title: "An internal error has occurred. Please contact the system admin.");
            }
        }

        #endregion
    }
}
