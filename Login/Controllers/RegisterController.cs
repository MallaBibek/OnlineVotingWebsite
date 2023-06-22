using Login.Data;
using Login.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Login.Controllers
{
    public class RegisterController : Controller
    {
        private readonly LoginContext _context;
        public RegisterController(LoginContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateUser()
        {
            return View();

        }
        [HttpPost]
        public IActionResult CreateUser(RegisterModel obj)
        {


            var ExistingUser = _context.Registers.FirstOrDefault(x => x.UserName == obj.UserName);

            if (ExistingUser is not null)
            {
                ViewBag.Message = "Username already taken.";
                return View();

            }
            else
            {
                using var sha256 = SHA256.Create();
                byte[] passwordBytes = Encoding.UTF8.GetBytes(obj.Password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                string hashedPassword = Convert.ToBase64String(hashBytes);
                obj.Password = hashedPassword;
                ViewBag.message = "Registered Successfully";
                _context.Registers.Add(obj);
                _context.SaveChanges();
                return View();

            }
        }
    }
}
