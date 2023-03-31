using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaOlcar.Models;
using SistemaOlcar.Models.TableViewModel;

namespace SistemaOlcar.Controllers
{
    public class HomeController : Controller
    {
        OLCAREntities d = new OLCAREntities();

        [Error(Roles = "Cajero")]
        [Authorize(Roles = "Cajero")]
        public ActionResult DashboardCajero()
        {
            return View();
        }

        [Error(Roles = "Administrador")]
        [Authorize(Roles = "Administrador")]
        public ActionResult DashboardAdmin()
        {
            return View();
        }

        [Error(Roles = "Operario de Almacén")]
        [Authorize(Roles = "Operario de Almacén")]
        public ActionResult DashboardAlmacen()
        {
            return View();
        }

        public JsonResult ReporteVentas()
        {
            List<SP_RetornaVentas_Result> objLista = new List<SP_RetornaVentas_Result>();

            using(OLCAREntities db = new OLCAREntities())
            {
                objLista = db.SP_RetornaVentas().ToList();
            }
            return Json(objLista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TopProductosVendidos()
        {
            List<SP_RetornaTopProductos_Result> objLista = new List<SP_RetornaTopProductos_Result>();

            using (OLCAREntities db = new OLCAREntities())
            {
                objLista = db.SP_RetornaTopProductos().ToList();
            }
            return Json(objLista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RetornaGananciaDia()
        {
            return Json(d.SP_GananciaDia(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult RetornaCantidadVentas()
        {
            return Json(d.SP_CantidadVentasDia(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult RetornaCantProdVendidos()
        {
            return Json(d.SP_CantidadProductosDia(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReporteGananciasDiarias()
        {
            List<SP_RetornaGananciasDiarias_Result> objLista = new List<SP_RetornaGananciasDiarias_Result>();

            using (OLCAREntities db = new OLCAREntities())
            {
                objLista = db.SP_RetornaGananciasDiarias().ToList();
            }
            return Json(objLista, JsonRequestBehavior.AllowGet);
        }

    }
}