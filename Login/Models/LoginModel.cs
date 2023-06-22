using System.ComponentModel.DataAnnotations;

namespace Login.Models
{
    public class LoginModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
