using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.DTOs.Recipe
{
    public class CreateRecipeDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }
        public List<int> IngredientIds { get; set; } = new List<int>();
    }
}