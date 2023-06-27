using Login.Data;
using Login.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Text;

namespace Login.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginContext _context;
        public LoginController( LoginContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(RegisterModel registerr,LoginModel Logiii)
        {
            var AuthenticatedUser = _context.Registers.FirstOrDefault(u => u.UserName == registerr.UserName);
            using var sha256 = SHA256.Create();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(registerr.Password);
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);
            string hashedPassword = Convert.ToBase64String(hashBytes);
            registerr.Password = hashedPassword;
       


            if (AuthenticatedUser is not null && registerr.Password == AuthenticatedUser.Password)
            {

                return RedirectToAction("Vote","Vote");
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password");
            }
            return View(registerr);
        }
    }
}
