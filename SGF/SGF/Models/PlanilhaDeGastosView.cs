using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGF.Models
{
    public class PlanilhaDeGastosView
    {
        public Usuario user { get; set; }
        public String ano { get; set; }
        public List<Renda> rendas { get; set; }
      
        

    }

    
}