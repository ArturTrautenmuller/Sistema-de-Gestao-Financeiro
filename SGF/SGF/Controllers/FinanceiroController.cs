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
            Session["Saldo"] = user.saldo;
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

        [HttpGet]
        public ActionResult RemoverCategoriaDireto()
        {
            int id = Convert.ToInt32(Request.QueryString["cat"]);
            TipoGasto categoria = LAD.TipoGastoLAD.Pesquisar.id(id);
            LAD.TipoGastoLAD.delete(categoria);

            return RedirectToAction("PlanilhaDeGastos", "Financeiro");
        }

        [HttpGet]
        public ActionResult trocarNomeCategoria() {
            int id = Convert.ToInt32(Request.QueryString["cat"]);
            String nome = Request.QueryString["nome"];
            LAD.TipoGastoLAD.atualizar(id, nome);

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

        [HttpGet]
        public ActionResult AdicionarCategoriaDireto()
        {
            TipoGasto categoria = new TipoGasto();
            categoria.nome = Request.QueryString["cat"];
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
            Gasto novoGasto = LAD.GastoLAD.Pesquisar.tudo((int)gasto.ano,(int)gasto.mes,(int)gasto.tipoGasto_id);
            return RedirectToAction("EditGasto", "Financeiro",novoGasto);
        }

        public ActionResult removerItem(Item item)
        {
            Gasto gasto = LAD.GastoLAD.Pesquisar.pesquisar((int)item.gasto_id);
            double novoValor = (double)gasto.valor - (double)item.valor;
            LAD.GastoLAD.atualizar(novoValor, gasto.id);

            LAD.UsuarioLAD.Atualizar.addSaldo((int)Session["User"], (double)item.valor);
            Session["Saldo"] = LAD.UsuarioLAD.Pesquisar.id((int)Session["User"]).saldo;

            LAD.ItemLAD.delete(item);
            gasto = LAD.GastoLAD.Pesquisar.pesquisar(gasto.id);
            return RedirectToAction("EditGasto", "Financeiro", gasto);
        }

        public ActionResult removerItem1(Item item)
        {
            Gasto gasto = LAD.GastoLAD.Pesquisar.pesquisar((int)item.gasto_id);
            double novoValor = (double)gasto.valor - (double)item.valor;
            LAD.GastoLAD.atualizar(novoValor, gasto.id);

            LAD.UsuarioLAD.Atualizar.addSaldo((int)Session["User"], (double)item.valor);
            Session["Saldo"] = LAD.UsuarioLAD.Pesquisar.id((int)Session["User"]).saldo;
            
           

            LAD.ItemLAD.delete(item);
            gasto = LAD.GastoLAD.Pesquisar.pesquisar(gasto.id);
            
            return RedirectToAction("PlanilhaDeGastosMensal", "Financeiro",gasto);
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

            LAD.UsuarioLAD.Atualizar.reduceSaldo((int)Session["User"], (double)item.valor);
            Session["Saldo"] = LAD.UsuarioLAD.Pesquisar.id((int)Session["User"]).saldo;

            LAD.ItemLAD.adicionar(item);
            Gasto gasto = LAD.GastoLAD.Pesquisar.pesquisar((int)item.gasto_id);
            double novoValor = (double)gasto.valor + (double)item.valor;
            LAD.GastoLAD.atualizar(novoValor, gasto.id);
            gasto = LAD.GastoLAD.Pesquisar.pesquisar((int)item.gasto_id);
            return RedirectToAction("EditGasto", "Financeiro", gasto);

        }

        [HttpPost]
        public ActionResult adicionarItem1(PlanilhaDeGastosMensalView pgvm)
        {
            if (pgvm.gastoId == "0") {
                Gasto inserirGasto = new Gasto()
                {
                    ano = (int)Session["Ano"],
                    mes = (int)Session["Mes"],
                    tipoGasto_id = Convert.ToInt32(pgvm.tipoGastoId),
                    valor = 0
                };

                LAD.GastoLAD.cadastrar(inserirGasto);
                pgvm.gastoId = Convert.ToString(LAD.GastoLAD.Pesquisar.data((int)Session["Ano"], (int)Session["Mes"], Convert.ToInt32(pgvm.tipoGastoId)).id);
            }


            Item item = new Item();
            item.gasto_id = Convert.ToInt32(pgvm.gastoId);
            item.nome = pgvm.itemNome;
            item.valor = Convert.ToDouble(pgvm.itemValor);
            item.dia = DateTime.Now.Day;
            if (pgvm.itemTipo == "Essencial" || pgvm.itemTipo == null) item.tipo = 0;
            if (pgvm.itemTipo == "Não-Essencial") item.tipo = 1;
            if (pgvm.itemTipo == "Investimento") item.tipo = 2;

            LAD.UsuarioLAD.Atualizar.reduceSaldo((int)Session["User"], (double)item.valor);
            Session["Saldo"] = LAD.UsuarioLAD.Pesquisar.id((int)Session["User"]).saldo;

            LAD.ItemLAD.adicionar(item);
            Gasto gasto = LAD.GastoLAD.Pesquisar.pesquisar((int)item.gasto_id);
            double novoValor = (double)gasto.valor + (double)item.valor;
            LAD.GastoLAD.atualizar(novoValor, gasto.id);
            gasto = LAD.GastoLAD.Pesquisar.pesquisar((int)item.gasto_id);
            return RedirectToAction("PlanilhaDeGastosMensal", "Financeiro", gasto);

        }

        public ActionResult removerGasto(Gasto gasto)
        {
            LAD.UsuarioLAD.Atualizar.addSaldo((int)Session["User"], (double)gasto.valor);
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
                double ultimoValor = (double)LAD.RendaLAD.Pesquisar.renda((int)Session["User"], i, (int)Session["Ano"]).valor;
                LAD.RendaLAD.Atualizar.atualizar((int)Session["Ano"], i, valor, (int)Session["User"]);
                LAD.UsuarioLAD.Atualizar.addSaldo((int)Session["User"], valor - ultimoValor);
            }

            return RedirectToAction("EditRenda", "Financeiro");
        }

        [HttpGet]
        public ActionResult setRenda() {
            int mes = Convert.ToInt32(Request.QueryString["mes"]);
            double valor = Convert.ToDouble(Request.QueryString["valor"]);
            double ultimoValor = (double)LAD.RendaLAD.Pesquisar.renda((int)Session["User"], mes, (int)Session["Ano"]).valor;

            LAD.RendaLAD.Atualizar.atualizar((int)Session["Ano"],mes,valor,(int)Session["User"]);
            LAD.UsuarioLAD.Atualizar.addSaldo((int)Session["User"], valor - ultimoValor);
            Session["Saldo"] = LAD.UsuarioLAD.Pesquisar.id((int)Session["User"]).saldo;


            return RedirectToAction("PlanilhaDeGastos", "Financeiro");
        }

        [HttpGet]
        public ActionResult setRenda1()
        {
            
            double valor = Convert.ToDouble(Request.QueryString["valor"]);
            double ultimoValor = (double)LAD.RendaLAD.Pesquisar.renda((int)Session["User"], (int)Session["Mes"], (int)Session["Ano"]).valor;

            LAD.RendaLAD.Atualizar.atualizar((int)Session["Ano"], (int)Session["Mes"], valor, (int)Session["User"]);
            LAD.UsuarioLAD.Atualizar.addSaldo((int)Session["User"], valor - ultimoValor);
            Session["Saldo"] = LAD.UsuarioLAD.Pesquisar.id((int)Session["User"]).saldo;

            return RedirectToAction("PlanilhaDeGastosMensal", "Financeiro", new Gasto() {id = 0, tipoGasto_id = 0 });
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
        [HttpGet]
        public ActionResult definirSaldo()
        {
            double saldo = Convert.ToDouble(Request.QueryString["saldo"]);
            LAD.UsuarioLAD.Atualizar.saldo((int)Session["User"], saldo);

            return RedirectToAction("PlanilhaDeGastos", "Financeiro");



        }

        public ActionResult PlanilhaDeGastosMensal(Gasto gasto) {
            if (Session["User"] == null) { return RedirectToAction("Index", "Home"); }
            List<TipoGasto> categorias = LAD.TipoGastoLAD.Pesquisar.userId((int)Session["User"]);
            PlanilhaDeGastosMensalView pgmv = new PlanilhaDeGastosMensalView();
            pgmv.categorias = categorias;

            Renda renda = LAD.RendaLAD.Pesquisar.renda((int)Session["user"], (int)Session["Mes"], (int)Session["Ano"]);
            pgmv.renda = renda;
            try
            {
                pgmv.catId = (int)gasto.tipoGasto_id;
            }
            catch { }

            return View(pgmv);

        }

        public ActionResult incrementarMes() {
            int mes = (int)Session["Mes"];
            if (mes < 12) {
                mes++;
            }
            else {
                mes = 1;
                int ano = (int)Session["Ano"];
                Session["Ano"] = (ano + 1);
            }

            Session["Mes"] = mes;


            return RedirectToAction("PlanilhaDeGastosMensal", "Financeiro", new Gasto() { tipoGasto_id = 0, id = 0 });
        }

        public ActionResult decrementarMes()
        {
            int mes = (int)Session["Mes"];
            if (mes > 1)
            {
                mes--;
            }
            else
            {
                mes = 12;
                int ano = (int)Session["Ano"];
                Session["Ano"] = (ano - 1);
            }

            Session["Mes"] = mes;


            return RedirectToAction("PlanilhaDeGastosMensal", "Financeiro", new Gasto() { tipoGasto_id = 0, id = 0 });
        }
        [HttpGet]
        public ActionResult editItem() {

            int id = Convert.ToInt32(Request.QueryString["id"]);
            String nome = Request.QueryString["nome"];
            int tipo = Convert.ToInt32(Request.QueryString["tipo"]);
            int dia = Convert.ToInt32(Request.QueryString["dia"]);
            double valor = Convert.ToDouble(Request.QueryString["valor"]);


            Item item = LAD.ItemLAD.pesquisar(id);
            double ultimoValor = (double)item.valor;
            LAD.ItemLAD.atualizar(id, valor, nome, dia, tipo);

            Gasto gasto = LAD.GastoLAD.Pesquisar.pesquisar((int)item.gasto_id);
            double gastoValor = (int)gasto.valor;
            gastoValor += (valor - ultimoValor);
            LAD.GastoLAD.atualizar(gastoValor,gasto.id);


            LAD.UsuarioLAD.Atualizar.addSaldo((int)Session["User"], (valor - ultimoValor));
            Session["Saldo"] = LAD.UsuarioLAD.Pesquisar.id((int)Session["User"]).saldo;
            

            return RedirectToAction("PlanilhaDeGastosMensal", "Financeiro",gasto);
        }

        public ActionResult EstatisticasMes() {
            if (Session["User"] == null) { return RedirectToAction("Index", "Home"); }
            EstatisticasMesView emv = new EstatisticasMesView();
            emv.categorias = LAD.TipoGastoLAD.Pesquisar.userId((int)Session["User"]);

            return View(emv);
        }

        [HttpGet]
        public FileContentResult ExportToExcel()
        {
            List<ExportCategorias> categorias = new List<ExportCategorias>();
            List<TipoGasto> cats = LAD.TipoGastoLAD.Pesquisar.userId((int)Session["User"]);
            List<Renda> rendas = LAD.RendaLAD.Pesquisar.ano((int)Session["Ano"],(int)Session["User"]);

            double[] TotalMes = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] salario = new double[12];
            double[] restante = new double[12];
            for (int i = 0; i < 12; i++) {
                var query = from renda in rendas where (renda.mes == i + 1 && renda.ano == (int)Session["Ano"]) select renda;
                try {
                    salario[i] = (double)query.ToList()[0].valor;
                }
                catch
                {
                    salario[i] = 0;
                }
            }

            categorias.Add(new ExportCategorias {Categoria = "Salário", Janeiro=salario[0],Fevereiro=salario[1],Março=salario[2],Abril=salario[3],Maio=salario[4],Junho=salario[5],Julho=salario[6],Agosto=salario[7],Setembro=salario[8],Outubro=salario[9],Novembro=salario[10],Dezembro=salario[11] });


            foreach (TipoGasto cat in cats) {
                ExportCategorias categoria = new ExportCategorias();
                categoria.Categoria = cat.nome;

                var query1 = from gasto in cat.Gasto where (gasto.mes == 1 && gasto.ano == (int)Session["Ano"]) select gasto;
                try
                {
                   
                    categoria.Janeiro = (double)query1.ToList()[0].valor;
                    TotalMes[0] += categoria.Janeiro;
                }
                catch {
                    categoria.Janeiro = 0;
                }

                var query2 = from gasto in cat.Gasto where (gasto.mes == 2 && gasto.ano == (int)Session["Ano"]) select gasto;
                try
                {
                    categoria.Fevereiro = (double)query2.ToList()[0].valor;
                    TotalMes[1] += categoria.Fevereiro;
                }
                catch
                {
                    categoria.Fevereiro = 0;
                }

                var query3 = from gasto in cat.Gasto where (gasto.mes == 3 && gasto.ano == (int)Session["Ano"]) select gasto;
                try
                {
                    categoria.Março = (double)query3.ToList()[0].valor;
                    TotalMes[2] += categoria.Março;
                }
                catch
                {
                    categoria.Março = 0;
                }

                var query4 = from gasto in cat.Gasto where (gasto.mes == 4 && gasto.ano == (int)Session["Ano"]) select gasto;
                try
                {
                    categoria.Abril = (double)query4.ToList()[0].valor;
                    TotalMes[3] += categoria.Abril;
                }
                catch
                {
                    categoria.Abril = 0;
                }

                var query5 = from gasto in cat.Gasto where (gasto.mes == 5 && gasto.ano == (int)Session["Ano"]) select gasto;
                try
                {
                    categoria.Maio = (double)query5.ToList()[0].valor;
                    TotalMes[4] += categoria.Maio;
                }
                catch
                {
                    categoria.Maio = 0;
                }
                var query6 = from gasto in cat.Gasto where (gasto.mes == 6 && gasto.ano == (int)Session["Ano"]) select gasto;
                try
                {
                    categoria.Junho = (double)query6.ToList()[0].valor;
                    TotalMes[5] += categoria.Junho;
                }
                catch
                {
                    categoria.Junho = 0;
                }

                var query7 = from gasto in cat.Gasto where (gasto.mes == 7 && gasto.ano == (int)Session["Ano"]) select gasto;
                try
                {
                    categoria.Julho = (double)query7.ToList()[0].valor;
                    TotalMes[6] += categoria.Julho;
                }
                catch
                {
                    categoria.Julho = 0;
                }

                var query8 = from gasto in cat.Gasto where (gasto.mes == 8 && gasto.ano == (int)Session["Ano"]) select gasto;
                try
                {
                    categoria.Agosto = (double)query8.ToList()[0].valor;
                    TotalMes[7] += categoria.Agosto;
                }
                catch
                {
                    categoria.Agosto = 0;
                }

                var query9 = from gasto in cat.Gasto where (gasto.mes == 9 && gasto.ano == (int)Session["Ano"]) select gasto;
                try
                {
                    categoria.Setembro = (double)query9.ToList()[0].valor;
                    TotalMes[8] += categoria.Setembro;
                }
                catch
                {
                    categoria.Setembro = 0;
                }

                var query10 = from gasto in cat.Gasto where (gasto.mes == 10 && gasto.ano == (int)Session["Ano"]) select gasto;
                try
                {
                    categoria.Outubro = (double)query10.ToList()[0].valor;
                    TotalMes[9] += categoria.Outubro;
                }
                catch
                {
                    categoria.Outubro = 0;
                }

                var query11 = from gasto in cat.Gasto where (gasto.mes == 11 && gasto.ano == (int)Session["Ano"]) select gasto;
                try
                {
                    categoria.Novembro = (double)query11.ToList()[0].valor;
                    TotalMes[10] += categoria.Novembro;
                }
                catch
                {
                    categoria.Novembro = 0;
                }

                var query12 = from gasto in cat.Gasto where (gasto.mes == 12 && gasto.ano == (int)Session["Ano"]) select gasto;
                try
                {
                    categoria.Dezembro = (double)query12.ToList()[0].valor;
                    TotalMes[11] += categoria.Dezembro;
                }
                catch
                {
                    categoria.Dezembro = 0;
                }
                categorias.Add(categoria);
            }
            

            categorias.Add(new ExportCategorias { Categoria = "Total", Janeiro = TotalMes[0], Fevereiro = TotalMes[1], Março = TotalMes[2], Abril = TotalMes[3], Maio = TotalMes[4], Junho = TotalMes[5], Julho = TotalMes[6], Agosto = TotalMes[7], Setembro = TotalMes[8], Outubro = TotalMes[9], Novembro = TotalMes[10], Dezembro = TotalMes[11] });
            for (int i = 0; i < 12; i++) {
                restante[i] = salario[i] - TotalMes[i];
            }
            categorias.Add(new ExportCategorias { Categoria = "Restante", Janeiro = restante[0], Fevereiro = restante[1], Março = restante[2], Abril = restante[3], Maio = restante[4], Junho = restante[5], Julho = restante[6], Agosto = restante[7], Setembro = restante[8], Outubro = restante[9], Novembro = restante[10], Dezembro = restante[11] });

            string[] columns = { "Categoria", "Janeiro", "Fevereiro","Março","Abril","Maio","Junho","Junlho","Agosto","Setembro","Outubro","Novembro","Dezembro" };
            byte[] filecontent = GerarTabela.ExportExcel(categorias, "Planilha", true, columns);
            return File(filecontent, GerarTabela.ExcelContentType, "Planilha_De_Gastos.xlsx");
        }

    }
}