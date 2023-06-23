using System.ComponentModel.DataAnnotations;

namespace Login.Models
{
    public class RegisterModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }


        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        [Required]
        public string UserName{ get; set; }

        /*[RegularExpression("[A-Z]+[ a-z 0-9]+[ ._%+-@]")]*/
        public string Password { get; set; }
    }
}
