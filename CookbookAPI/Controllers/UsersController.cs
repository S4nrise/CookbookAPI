using CookbookAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CookbookAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        public readonly List<User> Users = new List<User>
        {
            new User { Id = 1, Name = "Sanya", Password = "1234" },
            new User { Id = 2, Name = "Petya", Password = "2134" }
        };
    }
}
