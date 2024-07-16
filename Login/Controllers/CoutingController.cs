using Dapper;
using Login.Data;
using Login.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Login.Controllers
{
    public class CoutingController : Controller
    {
        private readonly LoginContext _context;
        private readonly DapperSql _dapperSql;
        public CoutingController(LoginContext AppDbContext)
        {
            _context = AppDbContext;
            _dapperSql = new DapperSql();
        }
        public ActionResult ExecuteStoredProc()

        {
            List<VotesCalculation> list = new(); 
            return View(list);

        }

        //    [HttpPost]
        //    public ActionResult ExecuteStoredProc(string partyValue)
        //    {
        //        var result = _context.Voteeee.FromSqlRaw("EXEC ToCount @Party", new SqlParameter("@Party", partyValue)).ToList().FirstOrDefault();
        //        var model = new VotesCalculation { VotesCount = result.VotesCount };
        //        return View(model);


        //}
        [HttpPost]
        public ActionResult ExecuteStoredProc(string partyValue)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@party", partyValue);

            _dapperSql.LogParametersToApplicationInsights("Counting", parameters);

            var result = _dapperSql.ExecuteStoredProcedure("Counting",
                reader => new VotesCalculation
                {
                    CountedVotes = Convert.ToInt32(reader["PartyCount"])
                },
                parameters
            );
            return View(result);
        }



    }
}