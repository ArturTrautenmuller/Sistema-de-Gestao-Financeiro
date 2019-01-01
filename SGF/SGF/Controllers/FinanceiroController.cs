using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SGF.Models;

namespace SGF.Controllers
{
    public class FinanceiroController : Controller
    {
        // GET: Financeiro
        public ActionResult PlanilhaDeGastos()
        {
            if (Session["User"] == null) { return RedirectToAction("Index", "Home"); }
            Usuario user = LAD.UsuarioLAD.Pesquisar.id((int)Session["user"]);
            user.senha = null;
            PlanilhaDeGastosView pgv = new PlanilhaDeGastosView();
            pgv.user = user;
            pgv.rendas = LAD.RendaLAD.Pesquisar.ano((int)Session["Ano"], (int)Session["User"]);
            return View(pgv);
        }

        public ActionResult EditGasto(Gasto gasto)
        {

            if (Session["User"] == null) { return RedirectToAction("Index", "Home"); }
            EditGastoView egv = new EditGastoView();
            egv.gasto = LAD.GastoLAD.Pesquisar.pesquisar(gasto.id);
            return View(egv);
        }

        public ActionResult EditCategorias()
        {
            if (Session["User"] == null) { return RedirectToAction("Index", "Home"); }
            Usuario user = LAD.UsuarioLAD.Pesquisar.id((int)Session["User"]);
            user.senha = null;
            EditCategoriaView ecv = new EditCategoriaView();
            ecv.tipoGasto = user.TipoGasto.ToList();
            return View(ecv);
        }

        public ActionResult RemoverCategoria(TipoGasto categoria)
        {
            LAD.TipoGastoLAD.delete(categoria);

            return RedirectToAction("PlanilhaDeGastos", "Financeiro");
        }

        [HttpPost]
        public ActionResult AdicionarCategoria(EditCategoriaView ecv)
        {
            TipoGasto categoria = new TipoGasto();
            categoria.nome = ecv.categoriaNome;
            categoria.usuario_id = (int)Session["User"];

            LAD.TipoGastoLAD.adicionar(categoria);
            return RedirectToAction("PlanilhaDeGastos", "Financeiro");
        }

        [HttpPost]
        public ActionResult setAno(PlanilhaDeGastosView pgv)
        {

            Session["Ano"] = Convert.ToInt32(pgv.ano);
            return RedirectToAction("PlanilhaDeGastos", "Financeiro");

        }

        public ActionResult incrementarAno()
        {

            int ano = (int)Session["Ano"];
            Session["Ano"] = (ano + 1);
            return RedirectToAction("PlanilhaDeGastos", "Financeiro");

        }

        public ActionResult decrementarAno()
        {

            int ano = (int)Session["Ano"];
            Session["Ano"] = (ano - 1);
            return RedirectToAction("PlanilhaDeGastos", "Financeiro");

        }


        public ActionResult criarGasto(Gasto gasto)
        {
            Gasto inserirGasto = new Gasto() { ano = gasto.ano, mes = gasto.mes, valor = 0, tipoGasto_id = gasto.tipoGasto_id };
            LAD.GastoLAD.cadastrar(inserirGasto);
            return RedirectToAction("PlanilhaDeGastos", "Financeiro");
        }

        public ActionResult removerItem(Item item)
        {
            Gasto gasto = LAD.GastoLAD.Pesquisar.pesquisar((int)item.gasto_id);
            double novoValor = (double)gasto.valor - (double)item.valor;
            LAD.GastoLAD.atualizar(novoValor, gasto.id);

            LAD.ItemLAD.delete(item);
            gasto = LAD.GastoLAD.Pesquisar.pesquisar(gasto.id);
            return RedirectToAction("EditGasto", "Financeiro", gasto);
        }

        [HttpPost]
        public ActionResult adicionarItem(EditGastoView egv)
        {
            Item item = new Item();
            item.gasto_id = (int)Session["GastoId"];
            item.nome = egv.itemNome;
            item.valor = Convert.ToDouble(egv.itemValor);
            item.dia = DateTime.Now.Day;
            if (egv.itemTipo == "Essencial" || egv.itemTipo == null) item.tipo = 0;
            if (egv.itemTipo == "Não-Essencial") item.tipo = 1;
            if (egv.itemTipo == "Investimento") item.tipo = 2;

            LAD.ItemLAD.adicionar(item);
            Gasto gasto = LAD.GastoLAD.Pesquisar.pesquisar((int)item.gasto_id);
            double novoValor = (double)gasto.valor + (double)item.valor;
            LAD.GastoLAD.atualizar(novoValor, gasto.id);
            gasto = LAD.GastoLAD.Pesquisar.pesquisar((int)item.gasto_id);
            return RedirectToAction("EditGasto", "Financeiro", gasto);

        }

        public ActionResult removerGasto(Gasto gasto)
        {
            LAD.GastoLAD.delete(gasto.id);
            return RedirectToAction("PlanilhaDeGastos", "Financeiro");

        }

        public ActionResult EditRenda()
        {

            if (Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            List<Renda> rendas = LAD.RendaLAD.Pesquisar.ano((int)Session["Ano"], (int)Session["User"]);
            if (rendas.Count() < 12)
            {
                LAD.RendaLAD.Cadastrar.ano((int)Session["Ano"], (int)Session["User"]);
                rendas = LAD.RendaLAD.Pesquisar.ano((int)Session["Ano"], (int)Session["User"]);
            }

            EditRendaView erv = new EditRendaView();
            erv.rendas = rendas;

            return View(erv);
        }

        [HttpPost]
        public ActionResult setRendas()
        {
            for (int i = 1; i <= 12; i++)
            {
                double valor = Convert.ToDouble(Request["m" + i]);
                LAD.RendaLAD.Atualizar.atualizar((int)Session["Ano"], i, valor, (int)Session["User"]);
            }

            return RedirectToAction("EditRenda", "Financeiro");
        }

        public ActionResult Estatisticas() {
            if (Session["User"] == null) {return RedirectToAction("Index", "Home"); }
            EstatisticasView ev = new EstatisticasView();
            ev.rendas = LAD.RendaLAD.Pesquisar.ano((int)Session["Ano"], (int)Session["User"]);
            List<TipoGasto> categorias = LAD.TipoGastoLAD.Pesquisar.userId((int)Session["User"]);
            ev.categorias = categorias;
            List<Gasto> gastos = new List<Gasto>();
            foreach (TipoGasto cat in categorias) {
                List<Gasto> catGastos = cat.Gasto.ToList();
                var query = from gasto in catGastos where (gasto.ano == (int)Session["Ano"]) select gasto;
                gastos.AddRange(query.ToList());
            }
            ev.gastos = gastos;


            return View(ev);
        }


    }
}