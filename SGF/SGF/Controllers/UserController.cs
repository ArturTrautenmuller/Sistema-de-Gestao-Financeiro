using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SGF.Models;

namespace SGF.Controllers
{
    public class UserController : Controller
    {

        public ActionResult UserPage()
        {
        
            return View();
        }

        public ActionResult Sair() {
            Session["User"] = null;
            Session["Nome"] = null;
           return RedirectToAction("Index", "Home");
        }

        public ActionResult PlanilhaDeGastos() {
            Usuario user = LAD.UsuarioLAD.Pesquisar.id((int)Session["user"]);
            user.senha = null;
            PlanilhaDeGastosView pgv = new PlanilhaDeGastosView();
            pgv.user = user;
            return View(pgv);
        }

        public ActionResult EditGasto(Gasto gasto) {
            
            EditGastoView egv = new EditGastoView();
            egv.gasto = LAD.GastoLAD.pesquisar(gasto.id);
            return View(egv);
        }

        public ActionResult EditCategorias()
        {
            Usuario user = LAD.UsuarioLAD.Pesquisar.id((int)Session["User"]);
            user.senha = null;
            EditCategoriaView ecv = new EditCategoriaView();
            ecv.tipoGasto = user.TipoGasto.ToList();
            return View(ecv);
        }

        public ActionResult RemoverCategoria(TipoGasto categoria) {
            LAD.TipoGastoLAD.delete(categoria);
            
            return RedirectToAction("PlanilhaDeGastos", "User");
        }

        [HttpPost]
        public ActionResult AdicionarCategoria(EditCategoriaView ecv) {
            TipoGasto categoria = new TipoGasto();
            categoria.nome = ecv.categoriaNome;
            categoria.usuario_id = (int)Session["User"];
           
            LAD.TipoGastoLAD.adicionar(categoria);
            return RedirectToAction("PlanilhaDeGastos", "User");
        }

        [HttpPost]
        public ActionResult setAno(PlanilhaDeGastosView pgv)
        {

            Session["Ano"] =  Convert.ToInt32(pgv.ano);
            return RedirectToAction("PlanilhaDeGastos","User");

        }

        public ActionResult criarGasto (Gasto gasto)
        {
            Gasto inserirGasto = new Gasto() { ano = gasto.ano, mes = gasto.mes, valor = 0, tipoGasto_id = gasto.tipoGasto_id };
            LAD.GastoLAD.cadastrar(inserirGasto);
            return RedirectToAction("PlanilhaDeGastos", "User");
        }

        public ActionResult removerItem(Item item) {
            Gasto gasto = LAD.GastoLAD.pesquisar((int)item.gasto_id);
            double novoValor = (double)gasto.valor - (double)item.valor;
            LAD.GastoLAD.atualizar(novoValor, gasto.id);
            
            LAD.ItemLAD.delete(item);
            gasto = LAD.GastoLAD.pesquisar(gasto.id);
            return RedirectToAction("EditGasto", "User",gasto);
        }

        [HttpPost]
        public ActionResult adicionarItem(EditGastoView egv)
        {
            Item item = new Item();
            item.gasto_id = (int)Session["GastoId"];
            item.nome = egv.itemNome;
            item.valor = Convert.ToDouble(egv.itemValor);
            LAD.ItemLAD.adicionar(item);
            Gasto gasto = LAD.GastoLAD.pesquisar((int)item.gasto_id);
            double novoValor = (double)gasto.valor + (double)item.valor;
            LAD.GastoLAD.atualizar(novoValor, gasto.id);
            gasto = LAD.GastoLAD.pesquisar((int)item.gasto_id);
            return RedirectToAction("EditGasto", "User", gasto);

        }
    }
}