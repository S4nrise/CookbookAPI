using AutoMapper;
using CookbookAPI.Contracts;
using CookbookAPI.Models;

namespace CookbookAPI.Configuration.Mappings
{
    public class IngredientMappingProfile : Profile
    {
        public IngredientMappingProfile()
        {
            CreateMap<IngredientInRecipeDto, IngredientInRecipe>();
        }
    }
}
