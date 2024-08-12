using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeApi.Models;

namespace RecipeApi.Interfaces
{
    public interface IIngredientRepository
    {
        Task<List<Ingredient>> GetIngredientsByIdsAsync(List<int> ids);
        Task AddIngredientsAsync(List<Ingredient> ingredients);
    }

}