using Login.Data;
using Login.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Login.Controllers
{
    public class VoteController : Controller
    {
        private readonly LoginContext _context;
        public VoteController(LoginContext context)
        {
                _context = context;
        }
        public IActionResult Vote()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Vote(VotesCalculation qw)
        {
            

            _context.Voteeee.Add(qw);
            var result =_context.SaveChanges();

            return RedirectToAction("Hello", "Home");   
          
        }
    }
}
