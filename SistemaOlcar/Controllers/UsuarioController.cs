using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaOlcar.Models;
using SistemaOlcar.Models.TableViewModel;

namespace SistemaOlcar.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Listar() //Listar Usuario
        {
            List<TableUsuario> oLstUusuario = new List<TableUsuario>();
            using (OLCAREntities d = new OLCAREntities())
            {
                d.Configuration.ProxyCreationEnabled = false;
                oLstUusuario = (from p in d.Usuario 
                                select new TableUsuario
                                {
                                    idUsuario = p.idUsuario,
                                    usuario = p.usuario1,
                                    rol = p.Rol.nombre,
                                    estado = p.estado
                                }
                                ).ToList();
            }
            return Json(new { data = oLstUusuario }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Obtener(int idUsuario) //Obtiene para editar
        {
            Usuario oUsuario = new Usuario();

            using (OLCAREntities db = new OLCAREntities())
            {
                db.Configuration.ProxyCreationEnabled = false; //Deshabilita carga diferida
                oUsuario = (from p in db.Usuario.Where(x => x.idUsuario == idUsuario)
                              select p).FirstOrDefault();
            }

            return Json(oUsuario, JsonRequestBehavior.AllowGet);
        }
    }
}