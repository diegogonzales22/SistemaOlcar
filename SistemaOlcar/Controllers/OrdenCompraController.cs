using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaOlcar.Models;
using SistemaOlcar.Models.TableViewModel;

namespace SistemaOlcar.Controllers
{
    public class OrdenCompraController : Controller
    {
        // GET: OrdenCompra
        public ActionResult Index() //Inicio
        {
            return View();
        }

        public JsonResult Listar() //Lista órdenes de compra
        {
            List<TableOrdenCompra> oLstOrden = new List<TableOrdenCompra>();
            using (OLCAREntities d = new OLCAREntities())
            {
                //d.Configuration.ProxyCreationEnabled = false;
                oLstOrden = (from p in d.OrdenCompra
                                select new TableOrdenCompra
                                {
                                    idOrden = p.idOrden,
                                    fechaRegistro = p.fechaRegistro,
                                    proveedor = p.Proveedor.nombre,
                                    situacion = p.situacion,
                                    costoTotal = p.costoTotal
                                }).ToList();
            }
            return Json(new { data = oLstOrden }, JsonRequestBehavior.AllowGet);
        }
    }
}