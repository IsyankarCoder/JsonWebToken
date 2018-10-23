using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace WebApplication12.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class BookController : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public IEnumerable<Book> Get()
        {
            int age=-1;
            var currentUser = HttpContext.User;
            var resultBooksList = new List<Book>
            {
               new Book { Author = "Ray Bradbury",Title = "Fahrenheit 451",AgeRestriction=false },
               new Book { Author = "Gabriel García Márquez", Title = "One Hundred years of Solitude",AgeRestriction=true },
               new Book { Author = "George Orwell", Title = "1984" ,AgeRestriction=true},
               new Book { Author = "Anais Nin", Title = "Delta of Venus",AgeRestriction=false }
            };

            if (currentUser.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                DateTime BirthDate = DateTime.Parse(currentUser.Claims.FirstOrDefault(d => d.Type == ClaimTypes.DateOfBirth).Value);
                age =DateTime.Today.Year - BirthDate.Year;
            }


            if (age < 18)
            {
                resultBooksList = resultBooksList.Where(b => !b.AgeRestriction).ToList();
            }


            return resultBooksList;
        }


        public class Book
        {
            public string Author { get; set; }
            public string Title { get; set; }
            public bool AgeRestriction { get; set; }

        }
    }
}