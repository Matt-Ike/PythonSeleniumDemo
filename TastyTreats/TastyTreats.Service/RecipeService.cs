using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TastyTreats.Model.DTO;
using TastyTreats.Model.Entities;
using TastyTreats.Types;
using TatyTreats.Repository;

namespace TastyTreats.Service
{
    public class RecipeService 
    {
        #region Fields
        private readonly RecipeRepo repo = new();
        #endregion

        #region Public Methods
        public Recipe AddRecipe(Recipe recipe)
        {
            if (ValidateRecipe(recipe))
                repo.AddRecipe(recipe);

            return recipe;
        }

        public List<RecipeDTO> GetAllWithDetails()
        {
            return repo.RetrieveAllWithDetails();
        }

        public async Task<List<RecipeDTO>> GetAllWithDetailsAsync()
        {
            return await repo.RetrieveAllWithDetailsAsync();
        }

        public async Task<RecipeDTO?> GetWithDetailsAsync(int recipId)
        {
            return await repo.GetWithDetailsAsync(recipId);
        }


        public Recipe Get(int id)
        {
            return repo.Retrieve(id);
        }

        public async Task<Recipe> GetAsync(int id)
        {
            return await repo.RetrieveAsync(id);
        }



        public RecipeReviewsDTO GetRecipeWithReviews(int recipeId)
        {
            return repo.RetrieveWithReviews(recipeId);
        }

        public async Task<RecipeReviewsDTO> GetRecipeWithReviewsAsync(int recipeId)
        {
            return await repo.RetrieveWithReviewsAsync(recipeId);
        }

        public Recipe UpdateRecipe(Recipe recipe)
        {
            if(ValidateRecipe(recipe))
                return repo.UpdateRecipe(recipe);

            return recipe;
        }

        public async Task<Recipe> UpdateRecipeAsync(Recipe recipe)
        {
            if (ValidateRecipe(recipe))
                return await repo.UpdateRecipeAsync(recipe);

            return recipe;
        }

        public void DeleteRecipe(int id)
        {
            repo.DeleteRecipe(id);
        }

        #endregion

        #region Private Methods
        private bool ValidateRecipe(Recipe r)
        {
            List<ValidationResult> results = new();

            Validator.TryValidateObject(r, new ValidationContext(r), results, true);

            foreach(ValidationResult result in results)
            {
                r.AddError(new(result.ErrorMessage, ErrorType.Model));
            }

            //Business Rules
            if (!CanChefAddRecipeOfType(r.ChefId, r.RecipeTypeId))
                r.AddError(new("Pastry chef can have only desserts.", ErrorType.Business));

            if(!ChefIdIsZero(r.ChefId))
                r.AddError(new("You must select a chef.", ErrorType.Business));

            if (!TypeIdIsZero(r.ChefId))
                r.AddError(new("You must select a recipe type.", ErrorType.Business));

            return r.Errors.Count == 0;
        }

        private bool CanChefAddRecipeOfType(int chefId, int recipeTypeId)
        {
            int? chefTypeId = repo.GetChefTypeId(chefId);

            if((chefTypeId == 4 && recipeTypeId == 3) || chefTypeId != 4)
                return true;

            return false;
        }

        private bool ChefIdIsZero(int chefId)
        {
            if(chefId == 0) return false;

            return true;
        }

        private bool TypeIdIsZero(int recipeTypeId)
        {
            if (recipeTypeId == 0) return false;

            return true;
        }
        #endregion
    }
}
