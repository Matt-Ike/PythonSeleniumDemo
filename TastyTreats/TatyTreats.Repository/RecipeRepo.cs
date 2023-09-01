using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TastyTreats.Model.DTO;
using TastyTreats.Model.Entities;
using TastyTreats.Types;

namespace TatyTreats.Repository
{
    public class RecipeRepo
    {
        #region Fields
        private readonly DataAccess db = new();
        #endregion

        #region Public Methods

        public bool AddRecipe(Recipe recipe)
        {
            List<ParmStruct> parms = new()
            {
                new("@recipeIdOut", SqlDbType.Int, recipe.RecipeId, 0, ParameterDirection.Output),
                new("@title", SqlDbType.NVarChar, recipe.Title, -1),
                new("@chefId", SqlDbType.Int, recipe.ChefId),
                new("@yield", SqlDbType.Int, recipe.Yield),
                new("@dateAdded", SqlDbType.DateTime2, recipe.DateAdded),
                new("@archived", SqlDbType.Bit, recipe.Archived),
                new("@recipeTypeId", SqlDbType.Int, recipe.RecipeTypeId),
            };

            if(db.ExecuteNonQuery("spAddRecipe", parms) > 0)
            {
                recipe.RecipeId = (int)parms.Where(p => p.Name == "@recipeIdOut").FirstOrDefault().Value;
                return true;
            }
            else
            {
                throw new DataException("There was an issue adding the recipe to the database");
            }
        }

        public List<RecipeDTO> RetrieveAllWithDetails()
        {
            DataTable dt = db.Execute("spGetAllRecipesWithDetails");

            return dt.AsEnumerable().Select(row => new RecipeDTO
            {
                RecipeId = Convert.ToInt32(row["RecipeId"]),
                Title = row["Title"].ToString(),
                ChefId = Convert.ToInt32(row["ChefId"]),
                Yield = Convert.ToInt32(row["Yield"]),
                DateAdded = Convert.ToDateTime(row["DateAdded"]),
                Archived = Convert.ToBoolean(row["Archived"]),
                RecipeTypeId = Convert.ToInt32(row["RecipeTypeId"]),
                ChefFirstName = row["FirstName"].ToString(),
                ChefLastName = row["LastName"].ToString(),
                ChefTypeId = Convert.ToInt32(row["ChefTypeId"]),
                Name = row["Name"].ToString()
            }).ToList();
        }

        public async Task<List<RecipeDTO>> RetrieveAllWithDetailsAsync()
        {
            DataTable dt = await db.ExecuteAsync("spGetAllRecipesWithDetails");

            return dt.AsEnumerable().Select(row => new RecipeDTO
            {
                RecipeId = Convert.ToInt32(row["RecipeId"]),
                Title = row["Title"].ToString(),
                ChefId = Convert.ToInt32(row["ChefId"]),
                Yield = Convert.ToInt32(row["Yield"]),
                DateAdded = Convert.ToDateTime(row["DateAdded"]),
                Archived = Convert.ToBoolean(row["Archived"]),
                RecipeTypeId = Convert.ToInt32(row["RecipeTypeId"]),
                ChefFirstName = row["FirstName"].ToString(),
                ChefLastName = row["LastName"].ToString(),
                ChefTypeId = Convert.ToInt32(row["ChefTypeId"]),
                Name = row["Name"].ToString()
            }).ToList();
        }

        public async Task<RecipeDTO?> GetWithDetailsAsync(int recipeId)
        {
            DataTable dt = await db.ExecuteAsync("spGetRecipesWithDetails", new List<ParmStruct> { new("@ArtistId", SqlDbType.Int, recipeId) });

            return dt.Rows.Count > 0 ? PopulateRecipeDTO(dt.Rows[0]) : null;
        }

        public Recipe? Retrieve(int id)
        {
            DataTable dt = db.Execute("spGetRecipe", new List<ParmStruct> { new ParmStruct("@RecipeId", SqlDbType.Int, id) });

            return dt.Rows.Count > 0 ? PopulateRecipe(dt.Rows[0]) : null;
        }

        public async Task<Recipe?> RetrieveAsync(int id)
        {
            DataTable dt = await db.ExecuteAsync("spGetRecipe", new List<ParmStruct> { new ParmStruct("@RecipeId", SqlDbType.Int, id) });

            return dt.Rows.Count > 0 ? PopulateRecipe(dt.Rows[0]) : null;
        }

        public Recipe UpdateRecipe(Recipe r)
        {
            List<ParmStruct> parms = new()
            {
                new("@RecipeId", SqlDbType.Int, r.RecipeId),
                new("@Title", SqlDbType.NVarChar, r.Title),
                new("@ChefId", SqlDbType.Int, r.ChefId),
                new("@Yield", SqlDbType.Int, r.Yield),
                new("@DateAdded", SqlDbType.DateTime2, r.DateAdded),
                new("@Archived", SqlDbType.Bit, r.Archived),
                new("@RecipeTypeId", SqlDbType.Int, r.RecipeTypeId),
            };

            db.ExecuteNonQuery("spUpdateRecipe", parms);

            return r;
        }

        public async Task<Recipe> UpdateRecipeAsync(Recipe r)
        {
            List<ParmStruct> parms = new()
            {
                new("@RecipeId", SqlDbType.Int, r.RecipeId),
                new("@Title", SqlDbType.NVarChar, r.Title),
                new("@ChefId", SqlDbType.Int, r.ChefId),
                new("@Yield", SqlDbType.Int, r.Yield),
                new("@DateAdded", SqlDbType.DateTime2, r.DateAdded),
                new("@Archived", SqlDbType.Bit, r.Archived),
                new("@RecipeTypeId", SqlDbType.Int, r.RecipeTypeId),
            };

            await db.ExecuteNonQueryAsync("spUpdateRecipe", parms);

            return r;
        }

        public int? GetChefTypeId(int chefID)
        {
            List<ParmStruct> parms = new()
            {
                new("@chefId", SqlDbType.Int, chefID)
            };

            string sql = "SELECT ChefTypeId FROM Chef WHERE ChefId = @chefId";
            object returnVal = db.ExecuteScalar(sql, parms, CommandType.Text);
            return returnVal != null ? Convert.ToInt32(returnVal) : null;
        }

        public async Task<int?> GetChefTypeIdAsync(int chefID)
        {
            List<ParmStruct> parms = new()
            {
                new("@chefId", SqlDbType.Int, chefID)
            };

            string sql = "SELECT ChefTypeId FROM Chef WHERE ChefId = @chefId";
            object returnVal = await db.ExecuteScalarAsync(sql, parms, CommandType.Text);
            return returnVal != null ? Convert.ToInt32(returnVal) : null;
        }

        public int? GetRecipeTypeId(int recipeId)
        {
            List<ParmStruct> parms = new()
            {
                new("@recipeId", SqlDbType.Int, recipeId)
            };

            string sql = "SELECT RecipeTypeId FROM Recipe WHERE RecipeId = @recipeId";
            object returnVal = db.ExecuteScalar(sql, parms, CommandType.Text);
            return returnVal != null ? Convert.ToInt32(returnVal) : null;
        }

        public async Task<int?> GetRecipeTypeIdAsync(int recipeId)
        {
            List<ParmStruct> parms = new()
            {
                new("@recipeId", SqlDbType.Int, recipeId)
            };

            string sql = "SELECT RecipeTypeId FROM Recipe WHERE RecipeId = @recipeId";
            object returnVal = await db.ExecuteScalarAsync(sql, parms, CommandType.Text);
            return returnVal != null ? Convert.ToInt32(returnVal) : null;
        }

        public RecipeReviewsDTO RetrieveWithReviews(int recipeId)
        {
            DataTable dt = db.Execute("spGetRecipeWithReviews", new List<ParmStruct> { new ParmStruct("@RecipeId", SqlDbType.Int, recipeId) });

            if (dt.Rows.Count == 0)
                return null;

            RecipeReviewsDTO recipesWithReviews = new()
            {
                Details = new RecipeDTO
                {
                    RecipeId = Convert.ToInt32(dt.Rows[0]["RecipeId"]),
                    Title = dt.Rows[0]["Title"].ToString(),
                    ChefId = Convert.ToInt32(dt.Rows[0]["ChefId"]),
                    Yield = Convert.ToInt32(dt.Rows[0]["Yield"]),
                    DateAdded = Convert.ToDateTime(dt.Rows[0]["DateAdded"]),
                    Archived = Convert.ToBoolean(dt.Rows[0]["Archived"]),
                    RecipeTypeId = Convert.ToInt32(dt.Rows[0]["RecipeTypeId"]),
                    ChefFirstName = dt.Rows[0]["FirstName"].ToString(),
                    ChefLastName = dt.Rows[0]["LastName"].ToString(),
                    ChefTypeId = Convert.ToInt32(dt.Rows[0]["ChefTypeId"]),
                    Name = dt.Rows[0]["Name"].ToString()
                },
                Reviews = new List<ReviewDTO>()
            };

            foreach(DataRow row in dt.Rows)
            {
                //if the recipe has reviews
                if (row["ReviewId"] != DBNull.Value)
                {
                    recipesWithReviews.Reviews.Add(new ReviewDTO
                    {
                        ReviewId = Convert.ToInt32(row["ReviewId"]),
                        Comment= row["Comment"].ToString(),
                        Stars = Convert.ToInt32(row["Stars"]),
                        ReviewDate = Convert.ToDateTime(row["ReviewDate"])
                    });
                }
            }

            return recipesWithReviews;
        }

        public async Task<RecipeReviewsDTO> RetrieveWithReviewsAsync(int recipeId)
        {
            DataTable dt = await db.ExecuteAsync("spGetRecipeWithReviews", new List<ParmStruct> { new ParmStruct("@RecipeId", SqlDbType.Int, recipeId) });

            if (dt.Rows.Count == 0)
                return null;

            RecipeReviewsDTO recipesWithReviews = new()
            {
                Details = new RecipeDTO
                {
                    RecipeId = Convert.ToInt32(dt.Rows[0]["RecipeId"]),
                    Title = dt.Rows[0]["Title"].ToString(),
                    ChefId = Convert.ToInt32(dt.Rows[0]["ChefId"]),
                    Yield = Convert.ToInt32(dt.Rows[0]["Yield"]),
                    DateAdded = Convert.ToDateTime(dt.Rows[0]["DateAdded"]),
                    Archived = Convert.ToBoolean(dt.Rows[0]["Archived"]),
                    RecipeTypeId = Convert.ToInt32(dt.Rows[0]["RecipeTypeId"]),
                    ChefFirstName = dt.Rows[0]["FirstName"].ToString(),
                    ChefLastName = dt.Rows[0]["LastName"].ToString(),
                    ChefTypeId = Convert.ToInt32(dt.Rows[0]["ChefTypeId"]),
                    Name = dt.Rows[0]["Name"].ToString()
                },
                Reviews = new List<ReviewDTO>()
            };

            foreach (DataRow row in dt.Rows)
            {
                //if the recipe has reviews
                if (row["ReviewId"] != DBNull.Value)
                {
                    recipesWithReviews.Reviews.Add(new ReviewDTO
                    {
                        ReviewId = Convert.ToInt32(row["ReviewId"]),
                        Comment = row["Comment"].ToString(),
                        Stars = Convert.ToInt32(row["Stars"]),
                        ReviewDate = Convert.ToDateTime(row["ReviewDate"])
                    });
                }
            }

            return recipesWithReviews;
        }

        public void DeleteRecipe(int id)
        {
            db.ExecuteNonQuery("spDeleteRecipe", new List<ParmStruct> { new ParmStruct("@RecipeId", SqlDbType.Int, id) });
        }
        #endregion



        #region Private Methods 
        private Recipe PopulateRecipe(DataRow row)
        {
            return new()
            {
                RecipeId = Convert.ToInt32(row["RecipeId"]),
                Title = row["Title"].ToString(),
                ChefId = Convert.ToInt32(row["ChefId"]),
                Yield = Convert.ToInt32(row["Yield"]),
                DateAdded = Convert.ToDateTime(row["DateAdded"]),
                Archived = Convert.ToBoolean(row["Archived"]),
                RecipeTypeId = Convert.ToInt32(row["RecipeTypeId"])
            };
        }

        private RecipeDTO PopulateRecipeDTO(DataRow row)
        {
            return new()
            {
                RecipeId = Convert.ToInt32(row["RecipeId"]),
                Title = row["Title"].ToString(),
                ChefId = Convert.ToInt32(row["ChefId"]),
                Yield = Convert.ToInt32(row["Yield"]),
                DateAdded = Convert.ToDateTime(row["DateAdded"]),
                Archived = Convert.ToBoolean(row["Archived"]),
                RecipeTypeId = Convert.ToInt32(row["RecipeTypeId"]),
                ChefFirstName = row["FirstName"].ToString(),
                ChefLastName = row["LastName"].ToString(),
                ChefTypeId = Convert.ToInt32(row["ChefTypeId"]),
                RecipeType = Convert.ToInt32(row["RecipeTypeId"]),
                Name = row["Name"].ToString()
            };
        }
        #endregion
    }
}
