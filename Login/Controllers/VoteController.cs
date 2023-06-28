using Login.Data;
using Login.Models;
using Login.TokenServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace Login.Controllers
{
    public class VoteController : Controller
    {
        private readonly LoginContext _context;
        private readonly ITokenService _tokenservice;
        public VoteController(LoginContext context, ITokenService tokenservice)
        {
                _context = context;
            _tokenservice = tokenservice;
        }
        public IActionResult Vote()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Vote(VotesCalculation qw)
        {
            var userName = _tokenservice.GetUsernameFromToken();
            qw.VotedBy = userName;
            _context.Voteeee.Add(qw);
            var result =_context.SaveChanges();

            return RedirectToAction("Hello", "Home");   
          
        }
    }
}
