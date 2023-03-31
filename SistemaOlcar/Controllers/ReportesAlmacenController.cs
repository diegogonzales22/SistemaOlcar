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

        [Error(Roles = "Administrador,Operario de Almacén")]
        [Authorize(Roles = "Administrador,Operario de Almacén")]
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
                             //where p.estado == true
                             select new TableMarcaProducto
                             {
                                 idMarca = p.idMarca,
                                 nombre = p.nombre
                             }
                             ).ToList();
            }
            return Json(new { data = oLstMarca }, JsonRequestBehavior.AllowGet);
        }

        [Error(Roles = "Administrador,Operario de Almacén")]
        [Authorize(Roles = "Administrador,Operario de Almacén")]
        public ActionResult StockxMarca(int id)
        {
            var inventario = db.Producto.Where(x => x.MarcaProducto.idMarca == id && 
                                                x.estado == true).OrderBy(x => x.nombre);

            return View(inventario.ToList());
        }

        [Error(Roles = "Administrador,Operario de Almacén")]
        [Authorize(Roles = "Administrador,Operario de Almacén")]
        public ActionResult ListaProductos() //Lista de productos activos, inactivos y todos
        {
            return View();
        }

        public JsonResult ProductosActivos() //Listar Productos Activos
        {
            List<TableProducto> oLstProducto = new List<TableProducto>();
            using (OLCAREntities d = new OLCAREntities())
            {
                d.Configuration.ProxyCreationEnabled = false;
                oLstProducto = (from p in d.Producto
                                where p.estado == true
                                orderby p.idProducto ascending
                                select new TableProducto
                                {
                                    idProducto = p.idProducto,
                                    nombre = p.nombre,
                                    unidad = p.unidadMedida,
                                    codigoEAN = p.codigoEAN,
                                    precioVenta = (decimal)p.precioVenta,
                                    marca = p.MarcaProducto.nombre
                                }).ToList();
            }
            return Json(oLstProducto, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProductosInactivos() //Listar Productos Inactivos
        {
            List<TableProducto> oLstProducto = new List<TableProducto>();
            using (OLCAREntities d = new OLCAREntities())
            {
                d.Configuration.ProxyCreationEnabled = false;
                oLstProducto = (from p in d.Producto
                                where p.estado == false
                                orderby p.idProducto ascending
                                select new TableProducto
                                {
                                    idProducto = p.idProducto,
                                    nombre = p.nombre,
                                    unidad = p.unidadMedida,
                                    codigoEAN = p.codigoEAN,
                                    precioVenta = (decimal)p.precioVenta,
                                    marca = p.MarcaProducto.nombre
                                }).ToList();
            }
            return Json(oLstProducto, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProductosTodos() //Listar Productos Todos
        {
            List<TableProducto> oLstProducto = new List<TableProducto>();
            using (OLCAREntities d = new OLCAREntities())
            {
                d.Configuration.ProxyCreationEnabled = false;
                oLstProducto = (from p in d.Producto
                                orderby p.idProducto ascending
                                select new TableProducto
                                {
                                    idProducto = p.idProducto,
                                    nombre = p.nombre,
                                    unidad = p.unidadMedida,
                                    codigoEAN = p.codigoEAN,
                                    precioVenta = (decimal)p.precioVenta,
                                    marca = p.MarcaProducto.nombre
                                }).ToList();
            }
            return Json(oLstProducto, JsonRequestBehavior.AllowGet);
        }

    }
}