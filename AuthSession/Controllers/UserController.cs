using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthSession.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace AuthSession.Controllers
{
    [Route("api")]
    public class UserController : Controller
    {
        AuthSessionContext db = new AuthSessionContext();

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
                    if (loggedinUser.Password.Equals(password))
                    {
                        return Ok(true);
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
                db.User.Add(signupUser);
                db.SaveChanges();
                return Ok();

            }
            catch (Exception ex)
            {

            }
            return BadRequest();
        }
    }
}