using Dapper;
using Login.Data;
using Login.Models;
using Microsoft.AspNetCore.Mvc;


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
            //string sql = "Insert into Voteeee (VotesCount) values (@partyValue)";
            string sql = "Counting";
            var response = _dapperSql.LoadSPDataListWithParam<VotesCalculation>(sql, parameters);

            

            return View(response);
        }


    }
}