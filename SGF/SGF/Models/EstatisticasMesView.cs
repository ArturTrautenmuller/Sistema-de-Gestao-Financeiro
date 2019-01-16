using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGF.Models
{
    public class EstatisticasMesView
    {
        public List<TipoGasto> categorias { get; set; }
        public Renda renda { get; set; }
    }
}