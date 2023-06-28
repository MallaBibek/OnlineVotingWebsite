using Login.Models;

namespace Login.TokenServices
{
    public interface ITokenService
    {
        string CreateToken(RegisterModel user);
        string GetUsernameFromToken();
    }
}
