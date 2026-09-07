using AutoMapper;
using CookbookAPI.Contracts;
using CookbookAPI.Models;

namespace CookbookAPI.Configuration.Mappings
{
    public class RecipeMappingProfile : Profile
    {
        public RecipeMappingProfile()
        {
            CreateMap<Recipe, RecipeVm>()
                .ForCtorParam(nameof(Recipe.Rating), opt => opt.MapFrom(src => src.Rating.Count == 0 ? 0 : src.Rating.Average(v => v.Value)));

            CreateMap<CreateRecipeDto, Recipe>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest=>dest.Ingredients,opt=>opt.MapFrom(src=>src.IngredientsInRecipeDto))
                .ForMember(dest=>dest.Rating,opt=>opt.Ignore());

            CreateMap<UpdateRecipeDto, Recipe>()
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.IngredientsInRecipeDto))
                .ForMember(dest => dest.Rating, opt => opt.Ignore());
        }
    }
}