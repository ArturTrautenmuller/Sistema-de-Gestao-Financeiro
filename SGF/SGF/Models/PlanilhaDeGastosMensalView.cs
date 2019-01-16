using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGF.Models
{
    public class PlanilhaDeGastosMensalView
    {
        
        public List<TipoGasto> categorias { get; set; }
        public int catId { get; set; }
        public String itemNome { get; set; }
        public String itemValor { get; set; }
        public String itemTipo { get; set; }
        public String gastoId { get; set; }
        public String tipoGastoId { get; set; }
        public Renda renda { get; set; }
    }
}