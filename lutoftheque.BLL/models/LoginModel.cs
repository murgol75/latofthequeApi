using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lutoftheque.bll.models
{
    public class LoginModel
    {
        private string _nickname;
        private string _password;


        [Required]
        public string nickname
        {
            get
            {
                return _nickname;
            }
            set
            {
                _nickname = value;
            }
        }
        [Required]
        public string password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }
    }
}
