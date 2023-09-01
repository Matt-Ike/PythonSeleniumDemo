using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TastyTreats.Model.Entities;
using TastyTreats.Types;

namespace TatyTreats.Repository
{
    public class ReviewRepo
    {

        #region Fields
        private readonly DataAccess db = new();
        #endregion


        #region Public Methods
        public bool Add(Review review)
        {
            List<ParmStruct> parms = new()
            {
                new("@reviewIdOut", SqlDbType.Int, review.ReviewId, 0, ParameterDirection.Output),
                new("@comment", SqlDbType.NVarChar, review.Comment, -1),
                new("@stars", SqlDbType.Int, review.Stars),
                new("@reviewDate", SqlDbType.DateTime2, review.ReviewDate),
                new("@recipeId", SqlDbType.Int, review.RecipeId)
            };

            if (db.ExecuteNonQuery("spAddReview", parms) > 0)
            {
                review.ReviewId = (int)parms.Where(p => p.Name == "@reviewIdOut").FirstOrDefault().Value;
                return true;
            }
            else
            {
                throw new DataException("There was an issue adding the review to the database");
            }
                
        }

        public async Task<bool> AddAsync(Review review)
        {
            List<ParmStruct> parms = new()
            {
                new("@reviewIdOut", SqlDbType.Int, review.ReviewId, 0, ParameterDirection.Output),
                new("@comment", SqlDbType.NVarChar, review.Comment, -1),
                new("@stars", SqlDbType.Int, review.Stars),
                new("@reviewDate", SqlDbType.DateTime2, review.ReviewDate),
                new("@recipeId", SqlDbType.Int, review.RecipeId)
            };

            if (await db.ExecuteNonQueryAsync("spAddReview", parms) > 0)
            {
                review.ReviewId = (int)parms.Where(p => p.Name == "@reviewIdOut").FirstOrDefault().Value;
                return true;
            }
            else
            {
                throw new DataException("There was an issue adding the review to the database");
            }

        }
        #endregion
    }
}
