using System.ComponentModel.DataAnnotations;

namespace WebAdmin.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
