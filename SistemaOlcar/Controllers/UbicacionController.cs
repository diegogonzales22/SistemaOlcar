using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using SistemaOlcar.Models;

namespace SistemaOlcar.Controllers
{
    public class UbicacionController : Controller
    {
        // GET: Ubicacion
        [Error(Roles = "Operario de Almacén")]
        [Authorize(Roles = "Operario de Almacén")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Listar() //Listar Ubicaciones
        {
            List<Ubicacion> oLstUbicacion = new List<Ubicacion>();
            using (OLCAREntities d = new OLCAREntities())
            {
                d.Configuration.ProxyCreationEnabled = false;
                oLstUbicacion = (from p in d.Ubicacion select p).ToList();
            }
            return Json(new { data = oLstUbicacion }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Obtener(int idUbicacion) //Obtiene para editar
        {
            Ubicacion oUbicacion = new Ubicacion();

            using (OLCAREntities db = new OLCAREntities())
            {
                db.Configuration.ProxyCreationEnabled = false; //Deshabilita carga diferida
                oUbicacion = (from p in db.Ubicacion.Where(x => x.idUbicacion == idUbicacion)
                          select p).FirstOrDefault();
            }

            return Json(oUbicacion, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(Ubicacion oUbicacion) //Registra o edita una marca de producto
        {

            bool respuesta = true;
            try
            {

                if (oUbicacion.idUbicacion == 0)
                {
                    using (OLCAREntities db = new OLCAREntities())
                    {
                        db.Ubicacion.Add(oUbicacion);
                        db.SaveChanges();
                    }
                }
                else
                {
                    using (OLCAREntities db = new OLCAREntities())
                    {
                        Ubicacion tempMarca = (from p in db.Ubicacion
                                                   where p.idUbicacion == oUbicacion.idUbicacion
                                                   select p).FirstOrDefault();

                        tempMarca.descripcion = oUbicacion.descripcion;
                        tempMarca.estado = oUbicacion.estado;

                        db.SaveChanges();
                    }

                }
            }
            catch
            {
                respuesta = false;

            }

            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);

        }
    }
}