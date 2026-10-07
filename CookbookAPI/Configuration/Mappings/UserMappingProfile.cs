using AutoMapper;
using CookbookAPI.Contracts;
using CookbookAPI.Models;

namespace CookbookAPI.Configuration.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<SignUpDto, User>()
                .ForMember(dest=>dest.Id,opt=>opt.Ignore())
                .ForMember(dest=>dest.Recipes, opt=>opt.Ignore())
                .ForMember(dest=>dest.RecipeRating, opt=>opt.Ignore())
                .ForMember(dest => dest.Password, opt =>opt.Ignore());
        }
    }
}
