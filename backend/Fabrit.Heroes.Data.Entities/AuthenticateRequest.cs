using System.ComponentModel.DataAnnotations;

namespace Fabrit.Heroes.Data.Entities
{
    public class AuthenticateRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
