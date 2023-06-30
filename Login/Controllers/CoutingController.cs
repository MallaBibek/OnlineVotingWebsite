using Login.Data;
using Login.Models;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult ExecuteStoredProc(string partyValue)
        {

            var result = _context.Voteeee.FromSqlRaw($"EXEC testing").ToList();

            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
