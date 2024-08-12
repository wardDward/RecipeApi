using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecipeApi.Data;
using RecipeApi.Interfaces;
using RecipeApi.Models;

namespace RecipeApi.Repository
{
    public class IngredientRepository : IIngredientRepository
    {

        private readonly ApplicationDbContext _context;

        public IngredientRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddIngredientsAsync(List<Ingredient> ingredients)
        {
            await _context.Ingredients.AddRangeAsync(ingredients);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Ingredient>> GetIngredientsByIdsAsync(List<int> ids)
        {
            return await _context.Ingredients
                               .Where(i => ids.Contains(i.Id))
                               .ToListAsync();
        }

    }
}