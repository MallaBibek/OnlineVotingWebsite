using Login.Data;
using Login.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        public ActionResult ExecuteStoredProc()

        {
            return View();

        }

        [HttpPost]
        [HttpPost]
        public ActionResult ExecuteStoredProc(string partyValue)
        {
            var result = _context.Voteeee.FromSqlRaw("EXEC ToCount @Party", new SqlParameter("@Party", partyValue)).ToList().FirstOrDefault();
            var model = new VotesCalculation { VotesCount = result.VotesCount };
            return View(model);
        }
        public IActionResult Index()
        {
            return View();
        }
    }

    
}
