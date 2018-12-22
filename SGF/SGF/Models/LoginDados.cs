using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGF.Models
{
    public class LoginDados
    {
        public String email { get; set; }
        public String senha { get; set; }

        public String errorMessage { get; set; }
    }
}