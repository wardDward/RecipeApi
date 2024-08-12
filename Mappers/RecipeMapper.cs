using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeApi.DTOs.Recipe;
using RecipeApi.Models;

namespace RecipeApi.Mappers
{
    public static class RecipeMapper
    {
        public static Recipe MapCreateDtoToRecipe(CreateRecipeDto createDto)
        {
            return new Recipe
            {
                Name = createDto.Name,
                Description = createDto.Description,
                CategoryId = createDto.CategoryId,
                RecipeIngredients = createDto.IngredientIds
                .Select(id => new RecipeIngredient { IngredientId = id })
                .ToList()
            };
        }

        public static RecipeDto MapRecipeToDto(Recipe recipe)
        {
            return new RecipeDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                CategoryName = recipe.Category?.Name,
                IngredientNames = recipe.RecipeIngredients.Select(ri => ri.Ingredient.Name).ToList()
            };
        }
    }
}