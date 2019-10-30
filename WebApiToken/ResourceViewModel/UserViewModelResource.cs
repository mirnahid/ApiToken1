using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiToken.Enums;

namespace WebApiToken.ResourceViewModel
{
    public class UserViewModelResource
    {
        [Required(ErrorMessage ="istifade adi mutleq daxil edilmelidir")]
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage ="email adresi mutleq olmalidir")]
        [EmailAddress(ErrorMessage ="email adresiniz dogru formatta deyil")]
        public string  Email { get; set; }
        [Required(ErrorMessage ="parolu daxil etmek mutleqdir")]    
        public string Password { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Picture { get; set; }
        public string City { get; set; }
        public Gender Gender { get; set; }

    }
}
