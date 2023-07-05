using Login.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Login.TokenServices
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _Key;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        public TokenService( IHttpContextAccessor httpContextAccessor)
        {
            _Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("bd1a1ccf8095037f361a4d351e7c0de65f0776bfc2f478ea8d312c763bb6caca"));
            _HttpContextAccessor = httpContextAccessor;
        }
        public string CreateToken(RegisterModel user)
        {
            //throw new NotImplementedException();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName)
            };
            var creds = new SigningCredentials(_Key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string encodedToken = tokenHandler.WriteToken(token);

            

            //var cookieOptions = new CookieOptions
            //{
            //    Expires = DateTime.Now.AddDays(7),
            //    HttpOnly = true,
            //    Secure = true
            //};



            //_HttpContextAccessor.HttpContext.Response.Cookies.Append("TokenKey", encodedToken, cookieOptions);



            return encodedToken;
        }

        public string GetUsernameFromToken()
        {
            string encodedToken = _HttpContextAccessor.HttpContext.Request.Cookies["TokenKey"];



            if (!string.IsNullOrEmpty(encodedToken))
              {
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadToken(encodedToken) as JwtSecurityToken;



                if (token != null)
                {
                    //var username = token.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
                    var username = token.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
                    return username;
                }
            }
            return null;
        }
    }
}
