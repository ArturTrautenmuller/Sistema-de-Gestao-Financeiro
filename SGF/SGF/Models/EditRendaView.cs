using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGF.Models
{
    public class EditRendaView
    {
        public List<Renda> rendas { get; set; }
        public double[] valor = new double[12];
    }
}