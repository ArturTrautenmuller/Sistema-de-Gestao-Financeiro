using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGF.Models
{
    public class ModelToView
    {
        public static UsuarioView usuario (Usuario user) {
            UsuarioView uv = new UsuarioView();
            uv.id = user.id;
            uv.nome = user.nome;
            uv.senha = user.senha;
            uv.email = user.email;
            uv.TipoGasto = user.TipoGasto;

            return uv;
        }

    }
}