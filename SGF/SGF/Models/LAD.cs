using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGF.Models
{
    public class LAD
    {

        public static class UsuarioLAD
        {

            public static class Pesquisar {

                public static Usuario id(int id)
                {
                   
                    SGFEntities db = new SGFEntities();
                    Usuario user = db.Usuario.SingleOrDefault(x => x.id == id);
                    return user;
                }

                public static List<Usuario> nome(String nome) {
                    SGFEntities db = new SGFEntities();
                    return db.Usuario.SqlQuery("SELECT * FROM Usuario WHERE nome = '"+nome+"'").ToList<Usuario>();
                    
                }

                public static Usuario conferir(String email, String senha) {
                    SGFEntities db = new SGFEntities();
                    try
                    {
                        Usuario user = db.Usuario.SingleOrDefault(x => x.email == email);
                        if (user.senha == senha) return user;
                        else throw new Exception();
                    }

                    catch (Exception e) {
                        return null;
                    }
                }

                public static Usuario email(String email) {
                    SGFEntities db = new SGFEntities();
                    return  db.Usuario.SingleOrDefault(x => x.email == email);
                }


            }

            public static void cadastrar(Usuario user) {
                SGFEntities db = new SGFEntities();
                db.Usuario.Add(user);
                db.SaveChanges();
            }

            public static class Atualizar {
                public static void senha(int id, String senha)
                {
                    SGFEntities db = new SGFEntities();
                    Usuario currentUser = db.Usuario.First(x => x.id == id);
                    currentUser.senha = senha;
                    db.SaveChanges();

                }
            }
        }

        public static class TipoGastoLAD {
            public static void delete(TipoGasto categoria)
            {
                SGFEntities db = new SGFEntities();

                var cat = db.TipoGasto.SingleOrDefault(x => x.id == categoria.id);

                foreach(Gasto gasto in cat.Gasto.ToList())
                {
                    foreach (Item item in gasto.Item.ToList())
                    {
                        var item1 = db.Item.SingleOrDefault(x => x.id == item.id);
                        db.Item.Remove(item1);
                    }
                    var gasto1 = db.Gasto.SingleOrDefault(x => x.id == gasto.id);
                    db.Gasto.Remove(gasto1);

                }
               
                db.TipoGasto.Remove(cat);

                db.SaveChanges();


            }

            public static class Pesquisar {
                public static TipoGasto id(int id) {
                    SGFEntities db = new SGFEntities();
                    return db.TipoGasto.SingleOrDefault(x => x.id == id);
                }
            }

            public static void adicionar(TipoGasto categoria)
            {
                SGFEntities db = new SGFEntities();
                db.TipoGasto.Add(categoria);
                db.SaveChanges();

            }
        }

        public static class GastoLAD {
            public static Gasto pesquisar(int id)
            {
                SGFEntities db = new SGFEntities();
                return db.Gasto.SingleOrDefault(x => x.id == id);

            }

            public static void cadastrar(Gasto gasto) {
                SGFEntities db = new SGFEntities();
                db.Gasto.Add(gasto);
                db.SaveChanges();
            }

            public static void atualizar(double novoValor, int id) {
                SGFEntities db = new SGFEntities();
                Gasto gasto = db.Gasto.SingleOrDefault(x => x.id == id);
                gasto.valor = novoValor;
                db.SaveChanges();
            }
        }

        public static class ItemLAD {
            public static void delete(Item item) {
                SGFEntities db = new SGFEntities();
                var itemDelete = db.Item.SingleOrDefault(x => x.id == item.id);
                db.Item.Remove(itemDelete);
                db.SaveChanges();

            }

            public static void adicionar(Item item) {
                SGFEntities db = new SGFEntities();
                db.Item.Add(item);
                db.SaveChanges();
            }
        }


    }
}