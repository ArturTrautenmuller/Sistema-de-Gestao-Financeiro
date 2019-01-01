using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGF.Models
{
    public class EstatisticasView
    {
      public List<Renda> rendas { get; set; }
      public List<Gasto> gastos { get; set; }
      public List<TipoGasto> categorias { get; set; }

    }
}