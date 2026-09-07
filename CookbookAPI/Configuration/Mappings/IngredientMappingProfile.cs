using AutoMapper;
using CookbookAPI.Contracts;
using CookbookAPI.Models;

namespace CookbookAPI.Configuration.Mappings
{
    public class IngredientMappingProfile : Profile
    {
        public IngredientMappingProfile()
        {
            CreateMap<IngredientInRecipeDto, IngredientInRecipe>()
                .ForMember(dest => dest.Ingredient, opt => opt.Ignore())
                .ForMember(dest => dest.RecipeId, opt => opt.Ignore())
                .ForMember(dest => dest.Recipe, opt => opt.Ignore());
                //.ForCtorParam(nameof(IngredientInRecipe.IngredientId), opt=> opt.MapFrom(src=> src.IngredientId))
                //.ForCtorParam(nameof(IngredientInRecipe.Ingredient), opt=>opt.MapFrom(src=>src.IngredientId));
            CreateMap<IngredientInRecipe, IngredientsInRecipeVm>()
                .ForCtorParam(nameof(IngredientsInRecipeVm.IngredientId), opt => opt.MapFrom(src=> src.Ingredient.Id))
                .ForCtorParam(nameof(IngredientsInRecipeVm.Name), opt => opt.MapFrom(src => src.Ingredient.Name));
            CreateMap<Ingredient, IngredientVm>();
        }
    }
}