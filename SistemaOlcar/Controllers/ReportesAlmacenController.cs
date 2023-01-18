using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaOlcar.Models;
using SistemaOlcar.Models.TableViewModel;

namespace SistemaOlcar.Controllers
{
    public class ReportesAlmacenController : Controller
    {
        private OLCAREntities db = new OLCAREntities();


        public ActionResult InventarioxMarca()
        {
            return View();
        }

        public JsonResult ListarMarcas() //Listar Marca de Productos
        {
            List<TableMarcaProducto> oLstMarca = new List<TableMarcaProducto>();
            using (OLCAREntities d = new OLCAREntities())
            {
                d.Configuration.ProxyCreationEnabled = true;
                oLstMarca = (from p in d.MarcaProducto
                             where p.estado == true
                             select new TableMarcaProducto
                             {
                                 idMarca = p.idMarca,
                                 nombre = p.nombre
                             }
                             ).ToList();
            }
            return Json(new { data = oLstMarca }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StockxMarca(int id)
        {
            var inventario = db.Producto.Where(x => x.MarcaProducto.idMarca == id && 
                                                x.estado == true).OrderBy(x => x.nombre);

            return View(inventario.ToList());
        }

    }
}