using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationAPI.Models;
using RegistrationAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace RegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserContext _context;
        public AuthController(UserContext context)
        {
            _context = context;
        }



        [HttpPost]
        //Authenticate
        public ActionResult PostLogin(LoginViewModel u)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var passwordHash = Crypto.HashPassword(u.Password);

                    var userLog = _context.Users.SingleOrDefault(user => user.EmailAddress == u.EmailAddress);



                    if (userLog == null)
                    {
                        //Debug.WriteLine(Crypto.HashPassword(u.Password));
                        return Ok(new { ok = false, msg = "Password invalid" });
                    }
                    else
                    {
                        var validate = Crypto.VerifyHashedPassword(userLog.Password, u.Password);


                        //var userLog = _context.Users.SingleOrDefault(user => user.Email == u.Email && user.Password == u.Password );
                        if (!validate)
                        {
                            return Ok(new { ok = false, msg = "Password invalid" });

                        }

                        return Ok(new { ok = true, id = userLog.UserID });

                    }


                }
                catch (Exception)
                {
                    return Ok(new { ok = false, msg = "Invalid credentials" });

                }

            }

            return Ok(new { msg = "Error" });
        }



    }
}
