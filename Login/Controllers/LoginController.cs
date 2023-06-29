using Login.Data;
using Login.Models;
using Login.TokenServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Text;

namespace Login.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginContext _context;
        private readonly ITokenService _tokenservice;
      
        public LoginController(LoginContext context, ITokenService tokenservice)
        {
            _context = context;
            _tokenservice = tokenservice;
        
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
        public IActionResult Login(RegisterModel registerr)
        {

            var AuthenticatedUser = _context.Registers.FirstOrDefault(u => u.UserName == registerr.UserName);


            //if (username.VotedBy is not null) 
            //{
            //   return RedirectToAction("Hello", "Home");
            //}
            //else { 
            using var sha256 = SHA256.Create();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(registerr.Password);
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);
            string hashedPassword = Convert.ToBase64String(hashBytes);
            registerr.Password = hashedPassword;

            var token = _tokenservice.CreateToken(registerr);



            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                HttpOnly = true,
                Secure = true
            };
            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();


            httpContextAccessor.HttpContext.Response.Cookies.Append("TokenKey", token, cookieOptions);
            //   var username = _context.Voteeee.FirstOrDefault(x => x.VotedBy == registerr.UserName);
            var userName = _tokenservice.GetUsernameFromToken();
            var result = _context.Voteeee.FirstOrDefault(x => x.VotedBy == userName);

            if (result is null)
            {

                if (AuthenticatedUser is not null && registerr.Password == AuthenticatedUser.Password)
                {

                    return RedirectToAction("Vote", "Vote");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
                return View(registerr);
            }
            else {

                return RedirectToAction("Hello", "Home");

            }
        }
       
        //}
    }
}
