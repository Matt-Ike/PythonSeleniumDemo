using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TastyTreats.Model.DTO;
using TatyTreats.Repository;

namespace TastyTreats.Service
{
    public class LookupsService
    {
        private readonly LookupsRepo repo = new();

        public List<RecipeTypeLookupDTO> GetRecipeTypes()
        {
            return repo.RetrieveRecipeTypes();
        }

        public List<ChefLookupDTO> GetChefs()
        {
            return repo.RetrieveChefs();
        }
    }
}
