using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;



namespace WebApplication12.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration _config;
        public TokenController(IConfiguration config)
        {
            _config = config;
        }


        [AllowAnonymous]
        [HttpPost]

        public IActionResult CreateToken([FromBody] LoginModel login)
        {

            IActionResult response = Unauthorized();
            var user = CustomAuthenticate(login);
            if (user != null)
            {
                var tokenstring = BuildToken(user);
                response = new OkObjectResult(new { token = tokenstring });
            }

            return response;
        }

        private string BuildToken(UserModel userModel)
        {


            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub,userModel.Name),
                new Claim(JwtRegisteredClaimNames.Email,userModel.Email),
                new Claim(JwtRegisteredClaimNames.Birthdate,userModel.BirthDate.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                                            _config["Jwt:Issuer"],
                                            claims,
                                            expires: DateTime.Now.AddMinutes(30),
                                            signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public UserModel CustomAuthenticate(LoginModel lgmodel)
        {
            UserModel userModel = null;
            if (lgmodel.UserName == "vv" && lgmodel.PassWord == "123")
                userModel = new UserModel() { Email = "gencvolkan@gmail.com", Name = "volkan", BirthDate = DateTime.Now };
            return userModel;
        }


    }


   

    public class LoginModel
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }

    public class UserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public DateTime BirthDate { get; set; }
    }


}