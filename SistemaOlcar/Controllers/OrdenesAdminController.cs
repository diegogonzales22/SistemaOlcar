using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaOlcar.Models;
using SistemaOlcar.Models.TableViewModel;

namespace SistemaOlcar.Controllers
{
    public class OrdenesAdminController : Controller
    {
        OLCAREntities db = new OLCAREntities();
        private static Usuario SesionUsuario;

        // GET: OrdenesAdmin
        [Error(Roles = "Administrador")]
        [Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            return View();
        }

        [Error(Roles = "Administrador")]
        [Authorize(Roles = "Administrador")]
        public ActionResult Detalles(int? id) //Detalles
        {
            if (Session["Usuario"] != null)
            {
                SesionUsuario = (Usuario)Session["Usuario"];
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdenCompra ordenCompra = db.OrdenCompra.Find(id);
            if (ordenCompra == null)
            {
                return HttpNotFound();
            }
            return View(ordenCompra);
        }

        // POST: Aprobar Orden
        [HttpPost]
        public ActionResult AprobarOrden(int id)
        {
            using (var db = new OLCAREntities())
            {
                var oOrden = db.OrdenCompra.Find(id);
                oOrden.situacion = "Aprobada";
                oOrden.fechaAprobacion = DateTime.Now;
                oOrden.aprobadoPor = SesionUsuario.idUsuario;
                db.Entry(oOrden).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Json(new { success = true, message = "Se aprobó la órden con éxito" }, JsonRequestBehavior.AllowGet);
        }

        // POST: Denegar Orden
        [HttpPost]
        public ActionResult DenegarOrden(int id)
        {
            using (var db = new OLCAREntities())
            {
                var oOrden = db.OrdenCompra.Find(id);
                oOrden.situacion = "Denegada";
                oOrden.fechaAprobacion = DateTime.Now;
                oOrden.aprobadoPor = SesionUsuario.idUsuario;
                db.Entry(oOrden).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Json(new { success = true, message = "Se denegó la órden con éxito" }, JsonRequestBehavior.AllowGet);
        }

        //----DASHBOARD ADMIN

        public JsonResult OrdenesPendientes() //Lista órdenes de compra pendientes
        {
            List<TableOrdenCompra> oLstOrden = new List<TableOrdenCompra>();
            using (OLCAREntities d = new OLCAREntities())
            {
                oLstOrden = (from p in d.OrdenCompra
                             where p.situacion == "Por Aprobar"
                             select new TableOrdenCompra
                             {
                                 idOrden = p.idOrden,
                                 fechaRegistro = p.fechaRegistro.ToString(),
                                 proveedor = p.Proveedor.nombre,
                                 situacion = p.situacion,
                                 costoTotal = p.costoTotal,
                                 fechaVencimiento = p.fechaCaducidad.ToString(),
                                 observacion = p.observacion
                             }).ToList();
            }

            return Json(oLstOrden, JsonRequestBehavior.AllowGet);
        }
    }
}