using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGF.Models
{
    public class IndexView
    {

        public IndexView(List<Usuario> users) {
            this.users = users;
        }

        public List<Usuario> users;
    }
}