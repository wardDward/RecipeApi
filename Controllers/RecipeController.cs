using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecipeApi.DTOs.Recipe;
using RecipeApi.Interfaces;
using RecipeApi.Mappers;
using RecipeApi.Models;

namespace RecipeApi.Controllers
{
    [ApiController]
    [Route("api/recipes")]
    public class RecipeController : ControllerBase
    {

        private readonly IRecipeRepository _recipeRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IIngredientRepository _ingredientRepository;

        public RecipeController(IRecipeRepository recipeRepository, ICategoryRepository categoryRepository, IIngredientRepository ingredientRepository)
        {
            _recipeRepository = recipeRepository;
            _categoryRepository = categoryRepository;
            _ingredientRepository = ingredientRepository;
        }

    [HttpGet]
        public async Task<IActionResult> GetAll(){
            var recipes = await _recipeRepository.GetRecipesAsync();
            return Ok(recipes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeDto createRecipeDto)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(createRecipeDto.CategoryId);

            if (category == null)
            {
                category = new Category { Name = createRecipeDto.Name };
                await _categoryRepository.AddCategoryAsync(category);
            }

            var existingIngredients = await _ingredientRepository.GetIngredientsByIdsAsync(createRecipeDto.IngredientIds);
            var missingIngredientIds = createRecipeDto.IngredientIds.Except(existingIngredients.Select(i => i.Id)).ToList();

            if (missingIngredientIds.Any())
            {
                var newIngredients = missingIngredientIds.Select(id => new Ingredient { Id = id, Name = "New Ingredient" }).ToList();
                await _ingredientRepository.AddIngredientsAsync(newIngredients);
                existingIngredients.AddRange(newIngredients);
            }

            var recipe = RecipeMapper.MapCreateDtoToRecipe(createRecipeDto);
            recipe.Category = category;
            recipe.RecipeIngredients = existingIngredients.Select(i => new RecipeIngredient { IngredientId = i.Id, Ingredient = i }).ToList();

            await _recipeRepository.AddRecipeAsync(recipe);
            var recipeDto = RecipeMapper.MapRecipeToDto(recipe);

            return CreatedAtAction(nameof(GetRecipeById), new { id = recipeDto.Id }, recipeDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipeById(int id)
        {
            var recipe = await _recipeRepository.GetRecipeByIdAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            var recipeDto = RecipeMapper.MapRecipeToDto(recipe);
            return Ok(recipeDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] CreateRecipeDto updateDto)
        {
            var recipe = await _recipeRepository.GetRecipeByIdAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            recipe.Name = updateDto.Name;
            recipe.Description = updateDto.Description;
            recipe.CategoryId = updateDto.CategoryId;
            recipe.RecipeIngredients.Clear();
            recipe.RecipeIngredients = updateDto.IngredientIds.Select(id => new RecipeIngredient { IngredientId = id }).ToList();

            await _recipeRepository.UpdateRecipeAsync(recipe);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            await _recipeRepository.DeleteRecipeAsync(id);
            return NoContent();
        }

    }
}