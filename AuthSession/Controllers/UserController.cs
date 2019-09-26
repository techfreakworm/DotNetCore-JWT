using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using AuthSession.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace AuthSession.Controllers
{
    [Route("api")]
    public class UserController : Controller
    {
        AuthSessionContext db = new AuthSessionContext();

        private IConfiguration _config;

        public UserController(IConfiguration config)
        {
            _config = config;
        }

        //Login
        [Route("login")]
        [HttpPost]
        public IActionResult login([FromBody]dynamic loginData)
        {
            try
            {
                StringValues emailValue;
                StringValues passwordValue;
                Request.Headers.TryGetValue("email", out emailValue);
                Request.Headers.TryGetValue("password", out passwordValue);

                String email = emailValue.FirstOrDefault();
                String password = passwordValue.FirstOrDefault();
                
                User loggedinUser = db.User.Find(email);
                try
                {
                    if (BCrypt.Net.BCrypt.Verify(password, loggedinUser.Password))
                    {
                        String tokenString = GenerateJSONWebToken(loggedinUser);
                        return Ok(new { token = tokenString});
                    }
                }
                catch (Exception ex)
                {
                    return Unauthorized();
                }
            }
            catch(Exception ex)
            {
                
            }
            return BadRequest();
        }


        //Signup
        [Route("signup")]
        [HttpPost]
        public IActionResult singup([FromBody]User signupUser)
        {
            try
            {
                signupUser.Password = BCrypt.Net.BCrypt.HashPassword(signupUser.Password);
                db.User.Add(signupUser);
                db.SaveChanges();
                return Ok();

            }
            catch (Exception ex)
            {

            }
            return BadRequest();
        }

        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}