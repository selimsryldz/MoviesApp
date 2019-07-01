using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MoviesProject.ValueObjects
{
    public class RegisterViewModel
    {
        [DisplayName("Kullanıcı adı"), Required(ErrorMessage = "{0} alanı boş geçilemez."),
           StringLength(25, ErrorMessage = "{0} max. {1} karakter olabilir")]
        public string Username { get; set; }
        [DisplayName("E-posta"), Required(ErrorMessage = "{0} alanı boş geçilemez."),
            StringLength(70, ErrorMessage = "{0} max. {1} karakter olabilir"),
            EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta adresi giriniz")]
        public string EMail { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı boş geçilemez."),
            StringLength(25, ErrorMessage = "{0} max. {1} karakter olabilir")]
        public string Password { get; set; }
        [DisplayName("Şifre(Tekrar)"), Required(ErrorMessage = "{0} alanı boş geçilemez."),
             StringLength(25, ErrorMessage = "{0} max. {1} karakter olabilir"),
            Compare("Password", ErrorMessage = "{0} ile {1} uyuşmuyor")]
        public string Repassword { get; set; }
    }
}