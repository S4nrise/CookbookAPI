using AutoMapper;
using CookbookAPI.Contracts;
using CookbookAPI.Models;

namespace CookbookAPI.Configuration.Mapping
{
    public class RecipeMappingProfile : Profile
    {
        public RecipeMappingProfile()
        {
            CreateMap<Recipe, RecipeVm>()
                .ForCtorParam(nameof(Recipe.Rating), opt => opt.MapFrom(src => src.Rating.Count == 0 ? 0 : src.Rating.Average()));

            CreateMap<CreateRecipeDto, Recipe>();
            //.ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateRecipeDto, Recipe>();
        }
    }
}