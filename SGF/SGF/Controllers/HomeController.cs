using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SGF.Models;

namespace SGF.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            

            return View();
        }

        public ActionResult Cadastro(String errorMessage)
        {
            CadastroDados cd = new CadastroDados();
            cd.errorMessage = errorMessage;

            return View(cd);
        }

        [HttpPost]
        public ActionResult CadastroFinal(CadastroDados cadastro) {
            if(cadastro.email == null) return RedirectToAction("Cadastro", "Home", new { errorMessage = "O campo email é obrigatório" });
            if(cadastro.nome == null) return RedirectToAction("Cadastro", "Home", new { errorMessage = "O campo nome é obrigatório" });
            if(cadastro.senha == null) return RedirectToAction("Cadastro", "Home", new { errorMessage = "O campo senha é obrigatório"});
            if (LAD.UsuarioLAD.Pesquisar.email(cadastro.email) != null) return RedirectToAction("Cadastro","Home",new { errorMessage = "Email já cadastrado"});
            Usuario user = new Usuario();
            user.nome = cadastro.nome;
            user.email = cadastro.email;
            user.senha = cadastro.senha;
            LAD.UsuarioLAD.cadastrar(user);
            Session["User"] = LAD.UsuarioLAD.Pesquisar.conferir(user.email, user.senha).id;
            Session["Nome"] = user.nome;
            Session["Ano"] = 2018;

            return View(user);
        }

        public ActionResult Logar(String errorMessage) {
            LoginDados lg = new LoginDados();
            lg.errorMessage = errorMessage;
            
            return View(lg);
        }

        [HttpPost]
        public ActionResult Autenticar(LoginDados login)
        {
            if(login.email == null) return RedirectToAction("Logar", "Home", new { errorMessage = "Preencha o campo email" });
            Usuario user = LAD.UsuarioLAD.Pesquisar.conferir(login.email, login.senha);
            
            
            if (user == null) { return RedirectToAction("Logar", "Home", new { errorMessage = "Senha incorreta" }); }
            else{
               
                Session["User"] = user.id;
                Session["Nome"] = user.nome;
                return RedirectToAction("UserPage","User");
        }
        }

        
    }
}