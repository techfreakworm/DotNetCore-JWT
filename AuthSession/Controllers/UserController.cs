using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthSession.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthSession.Controllers
{
    [Route("api")]
    public class UserController : Controller
    {
        AuthSessionContext db = new AuthSessionContext();

        //Signup
        [Route("login")]
        [HttpPost]
        public IActionResult login([FromBody]dynamic loginData)
        {
            try
            {
                string email = loginData["email"];
                string password = loginData["password"];
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
    }
}