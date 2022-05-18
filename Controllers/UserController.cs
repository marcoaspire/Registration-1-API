using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrationAPI.Models;
using RegistrationAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace RegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;
        public UserController(UserContext context)
        {
            _context = context;
        }
        [Route("/users")]
        [HttpGet]
        public ActionResult Get(string email)
        {
            if (email != null)
            {
                return Ok(_context.Users
                    .Where(item => item.EmailAddress.Contains(email))
                    .Select(u => new
                    {
                        User = u,
                        Role = u.Employee.Role,
                        Salary = u.Employee.Salary
                    })
                    .ToList());

            }
            else
            //return Ok(new { results = _context.Users.ToList(), msg = "ALl users", });
            return Ok( _context.Users
                .Select(u => new
                {
                    User = u,
                    Role = u.Employee.Role,
                    Salary = u.Employee.Salary
                })
                //.Include(d => d.Employee)
                .ToList() );

        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public ActionResult GetUser(int id)
        {
            //return Ok(_context.Users.ToList());
            var u = _context.Users
                .Select(u => new
                {
                    User = u,
                    Role = u.Employee.Role,
                    Salary = u.Employee.Salary
                })
                .SingleOrDefault(user => user.User.UserID == id);
            return Ok(new
            {
                User = u
          
            });


        }

        
        private int GetUserByEmail(string email)
        {
            //return Ok(_context.Users.ToList());
            var u = _context.Users.SingleOrDefault(user => user.EmailAddress == email);
            return u.UserID;
        }

        private void createEmployee(int userID)
        {
            Employee e = new Employee
            {
                UserID = userID,
                Role= "USER_ROLE",
                Salary=0
            };
            _context.Employees.Add(e);
            _context.SaveChanges();
        }

        private bool deleteEmployee(int userID)
        {
            var e = _context.Employees.SingleOrDefault(e => e.UserID == userID);
            if (e == null)
            {

                return false ;
            }
            _context.Employees.Remove(e);
            _context.SaveChanges();
            return true;

        }

        //new user
        // POST api/<AuthController>/new
        [Route("new")]

        [HttpPost]
        public ActionResult Post(User user)
        {
            try
            {
                var passwordHash = Crypto.HashPassword(user.Password);
                user.Password = passwordHash;


                user.EnrolledDate = DateTime.Now.ToString();
                _context.Users.Add(user);
                _context.SaveChanges();
                
                Debug.WriteLine("NUEVO");

                //getID
                Debug.WriteLine(user.EmailAddress);
                int id = this.GetUserByEmail(user.EmailAddress);
                
                createEmployee(id);

                return Ok(new { ok = true, id = user.UserID, email = user.EmailAddress });

            }
            catch (Exception)
            {

                throw;
            }

        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserUpdate user)
        {
            Debug.WriteLine("ID recibido: " +  id);


            if (id != user.UserID)
            {
                return BadRequest();
            }

            var userDB = _context.Users.SingleOrDefault(u => u.UserID == id);

            if (userDB.FirstName != user.FirstName && user.FirstName != "" && user.FirstName != null)
            {
                userDB.FirstName = user.FirstName;
            }
            if (userDB.MiddleName != user.MiddleName && user.MiddleName != "" && user.MiddleName != null)
            {
                userDB.MiddleName = user.MiddleName;
            }
            if (userDB.LastName != user.LastName && user.LastName != "" && user.LastName != null)
            {
                userDB.LastName = user.LastName;
            }
            if (userDB.EmailAddress != user.EmailAddress && user.EmailAddress != "" && user.EmailAddress != null)
            {
                userDB.EmailAddress = user.EmailAddress;
            }
            if (userDB.MobilePhone != user.MobilePhone && user.MobilePhone != "" && user.MobilePhone != null)
            {
                userDB.MobilePhone = user.MobilePhone;
            }
            if (userDB.Last4DigitsSSN != user.Last4DigitsSSN)
            {
                userDB.Last4DigitsSSN = user.Last4DigitsSSN;
            }
            if (userDB.MaxLoginAttempt != user.MaxLoginAttempt)
            {
                userDB.MaxLoginAttempt = user.MaxLoginAttempt;
            }
            if (userDB.LastLogin != user.LastLogin && user.LastLogin != "" && user.LastLogin != null)
            {
                //userDB.LastLogin = user.LastLogin;
            }
            if (userDB.Status != user.Status && user.Status != "" && user.Status != null)
            {
                userDB.Status = user.Status;
            }
            if (userDB.EnrolledDate != user.EnrolledDate && user.EnrolledDate != "" && user.EnrolledDate != null)
            {
                //userDB.EnrolledDate = user.EnrolledDate;
            }
            if (userDB.Password != user.Password && user.Password != "" && user.Password != null)
            {
               // userDB.Password = user.Password;
            }



            _context.Entry(userDB).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return Ok(new { results = user, msg = "user updated" });
        }

        // DELETE: api/PaymentDetail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if (deleteEmployee(id))
            {

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok(new { msg = "user deleted" });
            }
            else
            {
                return BadRequest();
            }

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

                        return Ok(new { ok = true, id = userLog.UserID});

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
