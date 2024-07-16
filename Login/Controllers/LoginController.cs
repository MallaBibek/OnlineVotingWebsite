using Login.Data;
using Login.Models;
using Login.TokenServices;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;


namespace Login.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginContext _context;
        private readonly ITokenService _tokenservice;
        private readonly IConfiguration _configuration;
      
        public LoginController(LoginContext context, ITokenService tokenservice, IConfiguration configuration)
        {
            _context = context;
            _tokenservice = tokenservice;
            _configuration = configuration; 
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            string appkey = _configuration["recaptchaFlag"];
            ViewBag.appkey = appkey;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(RegisterModel registerr, string gRecaptchaResponse)
        {
            

            var isCaptchaValid = VerifyCaptcha(gRecaptchaResponse);
            if (isCaptchaValid is null)
            {
                ModelState.AddModelError("", "reCAPTCHA verification failed. Please try again.");
                return View(registerr);
            }
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
        private async Task< RecaptchaResponse> VerifyCaptcha(string gRecaptchaResponse)
        {
            RecaptchaResponse res = null;
            using (System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient())
            {
                var secretKey = "6LeL3mIpAAAAAHr0y-DQyIIupX6I5-9c3nVd2jjN";
                var values = new Dictionary<string,
                   string> {
                    {
                    "secret",
                    secretKey
                },
                {
                    "response",
                    gRecaptchaResponse
                }
            };


                // var response = await httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={gRecaptchaResponse}", null);

                var content = new System.Net.Http.FormUrlEncodedContent(values);
                var Response = httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content).Result;
                var responseString = Response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrWhiteSpace(responseString))
                {
                    res = JsonConvert.DeserializeObject<RecaptchaResponse>(responseString);
                    return res;
                }

            }
            return res;
        }
        }


        public class RecaptchaResponse
        {
            public bool Success { get; set; }
            public string ChallengeTs { get; set; }
            public string Hostname { get; set; }
            public List<string> ErrorCodes { get; set; }
        }

     
    }

