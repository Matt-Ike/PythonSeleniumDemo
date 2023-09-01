using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TastyTreats.Model.DTO;

namespace TatyTreats.Repository
{
    public class LookupsRepo
    {
        private readonly DataAccess db = new();

        public List<RecipeTypeLookupDTO> RetrieveRecipeTypes()
        {
            DataTable dt = db.Execute("spGetRecipeTypesForLookup");

            return dt.AsEnumerable().Select(row => new RecipeTypeLookupDTO
            {
                RecipeTypeId = Convert.ToInt32(row["RecipeTypeId"]),
                RecipeTypeName = row["Name"].ToString()
            }).ToList();
        }

        public async Task<List<RecipeTypeLookupDTO>> RetrieveRecipeTypesAsync()
        {
            DataTable dt = await db.ExecuteAsync("spGetRecipeTypesForLookup");

            return dt.AsEnumerable().Select(row => new RecipeTypeLookupDTO
            {
                RecipeTypeId = Convert.ToInt32(row["RecipeTypeId"]),
                RecipeTypeName = row["Name"].ToString()
            }).ToList();
        }



        public List<ChefLookupDTO> RetrieveChefs()
        {
            DataTable dt = db.Execute("spGetChefsForLookup");

            return dt.AsEnumerable().Select(row => new ChefLookupDTO
            {
                ChefId = Convert.ToInt32(row["ChefId"]),
                ChefName = row["ChefName"].ToString()
            }).ToList();
        }

        public async Task<List<ChefLookupDTO>> RetrieveChefsAsync()
        {
            DataTable dt = await db.ExecuteAsync("spGetChefsForLookup");

            return dt.AsEnumerable().Select(row => new ChefLookupDTO
            {
                ChefId = Convert.ToInt32(row["ChefId"]),
                ChefName = row["ChefName"].ToString()
            }).ToList();
        }
    }
}
