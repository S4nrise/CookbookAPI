using AutoMapper;
using CookbookAPI.Contracts;
using CookbookAPI.Models;

namespace CookbookAPI.Configuration.Mapping
{
    public class RecipeMappingProfile : Profile
    {
        public RecipeMappingProfile()
        {
            CreateMap<Recipe, RecipeVm>();

            CreateMap<CreateRecipeDto, Recipe>();
                //.ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateRecipeDto, Recipe>();
        }
    }
}
