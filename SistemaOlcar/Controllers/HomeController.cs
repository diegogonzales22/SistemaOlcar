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

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DashboardCajero()
        {
            return View();
        }

        public ActionResult DashboardAdmin()
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

    }
}