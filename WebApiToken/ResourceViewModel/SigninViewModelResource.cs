using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiToken.ResourceViewModel
{
    public class SigninViewModelResource
    {
        [Required(ErrorMessage ="email mutleq daxil edilmelidir")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage ="parol mutleq daxil edilmelidir")]
        [MinLength(4,ErrorMessage ="parol minimum 5 elementden ibaret ola biler")]
        public string Password { get; set; }
    }
}
