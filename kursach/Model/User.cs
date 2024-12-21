using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursach.Model
{
    public class User
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Пожалуйста, заполните поля с логином")]
        public string login { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Пожалуйста, введите пароль")]
        public string passwordHash { get; set; }  
        public string passwordSalt { get; set; }



    }
}
