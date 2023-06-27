using Login.Data;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Login.Controllers
{
    public class CoutingController : Controller
    {
        private readonly LoginContext _context;

        public CoutingController(LoginContext AppDbContext)
        {
            _context = AppDbContext;
        }

        public ActionResult CountedVote()
        {

            /*var query = "SELECT COUNT(NepaliCongress) AS CountedVote FROM [dbo].[Voteeee]";
            var countedVote = _context.Voteeee.FromSqlRaw(query).FirstOrDefault();*/
            var countedVote = _context.Voteeee.Count();

            ViewBag.CountedVote = countedVote;

           
            

            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
