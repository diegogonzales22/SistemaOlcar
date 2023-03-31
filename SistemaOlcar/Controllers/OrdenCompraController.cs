using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaOlcar.Models;
using SistemaOlcar.Models.TableViewModel;
using SistemaOlcar.Helpers;
using System.Net;
using System.Data.Entity;
using System.Net.Mail;
using System.Net.Mime;

namespace SistemaOlcar.Controllers
{
    public class OrdenCompraController : Controller
    {
        private OLCAREntities db = new OLCAREntities();
        private static Usuario SesionUsuario;
        static Utilitario help = new Utilitario();

        // GET: OrdenCompra
        [Error(Roles = "Operario de Almacén")]
        [Authorize(Roles = "Operario de Almacén")]
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
                                    fechaRegistro = p.fechaRegistro.ToString(),
                                    proveedor = p.Proveedor.nombre,
                                    situacion = p.situacion,
                                    costoTotal = p.costoTotal
                                }).ToList();
            }
            return Json(new { data = oLstOrden }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult HistorialCompras(int id) //Lista historial de compra
        {
            List<TableIngreso> oLstIngresos = new List<TableIngreso>();
            using (OLCAREntities d = new OLCAREntities())
            {
                //d.Configuration.ProxyCreationEnabled = false;
                oLstIngresos = (from p in d.DetalleIngreso
                                where p.idProducto == id
                                select new TableIngreso
                                {
                                     idIngreso = (int)p.idIngreso,
                                     fechaRegistro = p.fecha.ToString(),
                                     producto = p.Producto.nombre,
                                     cantidad = p.cantidad,
                                     descuento = p.descuento,
                                     costoSinIGV = p.precioUnidad,
                                     costoConIGV = decimal.Round(p.precioUnidad * 1.18m, 2),
                                     importeTotal = p.importeTotal
                                 }).ToList();
            }
            return Json(new { data = oLstIngresos }, JsonRequestBehavior.AllowGet);
        }

        //Obtener Productos como Json
        public JsonResult ObtenerProducto(int idProd)
        {
            Producto oProducto = new Producto();

            using (OLCAREntities db = new OLCAREntities())
            {
                db.Configuration.ProxyCreationEnabled = false; //Deshabilita carga diferida
                oProducto = (from p in db.Producto.Where(x => x.idProducto == idProd)
                             select p).FirstOrDefault();
            }

            return Json(oProducto, JsonRequestBehavior.AllowGet);
        }

        //Obtener Proveedores como Json
        public JsonResult ObtenerProveedor(int idPro)
        {
            Proveedor oproveedor = new Proveedor();

            using (OLCAREntities db = new OLCAREntities())
            {
                db.Configuration.ProxyCreationEnabled = false; //Deshabilita carga diferida
                oproveedor = (from p in db.Proveedor.Where(x => x.idProveedor == idPro)
                             select p).FirstOrDefault();
            }

            return Json(oproveedor, JsonRequestBehavior.AllowGet);
        }

        //Obtener Productos activos como Json
        public JsonResult ProductosActivos() //Listar Productos
        {
            List<TableProducto> oLstProducto = new List<TableProducto>();
            using (OLCAREntities d = new OLCAREntities())
            {
                d.Configuration.ProxyCreationEnabled = false;
                oLstProducto = (from p in d.Producto
                                where p.estado == true
                                select new TableProducto
                                {
                                    idProducto = p.idProducto,
                                    nombre = p.nombre,
                                    codigoEAN = p.codigoEAN,
                                    marca = p.MarcaProducto.nombre,
                                    stock = p.stock,
                                    stockMinimo = p.stockMinimo,
                                    stockMaximo = p.stockMaximo
                                }).ToList();
            }
            return Json(new { data = oLstProducto }, JsonRequestBehavior.AllowGet);
        }

        //Evaluar y obtener O.C.
        public JsonResult ObtenerOrden(int id)
        {
            OrdenCompra orden = new OrdenCompra();

            using (OLCAREntities db = new OLCAREntities())
            {
                db.Configuration.ProxyCreationEnabled = false; //Deshabilita carga diferida
                orden = (from p in db.OrdenCompra.Where(x => x.idOrden == id)
                             select p).FirstOrDefault();
            }

            return Json(orden, JsonRequestBehavior.AllowGet);
        }

        [Error(Roles = "Operario de Almacén")]
        [Authorize(Roles = "Operario de Almacén")]
        public ActionResult Registrar() //GET Orden de compra
        {
            if (Session["Usuario"] != null)
            {
                SesionUsuario = (Usuario)Session["Usuario"];
            }

            ViewBag.idProveedor = help.GetProveedor(); //Carga proveedor
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(TableOrdenCompra model)
        {
            try
            {
                using (OLCAREntities db = new OLCAREntities())
                {
                    OrdenCompra o = new OrdenCompra();
                    o.fechaRegistro = DateTime.Now;
                    o.creadoPor = SesionUsuario.idUsuario;
                    o.idProveedor = model.idProveedor;
                    o.situacion = "Por Aprobar";
                    o.totalNeto = model.totalNeto;
                    o.totalIGV = model.totalIGV;
                    o.costoTotal = model.costoTotal;
                    o.fechaAprobacion = null;
                    o.aprobadoPor = null;
                    o.fechaCaducidad = model.fechaCaducidad;
                    o.observacion = model.observacion;
                    db.OrdenCompra.Add(o);
                    db.SaveChanges();

                    foreach (var oD in model.DetalleOrden)
                    {
                        Models.DetalleOrden oDetalle = new Models.DetalleOrden();
                        decimal precioConDescuento = oD.cantidad* oD.precioUnidad * (oD.descuento / 100);
                        oDetalle.idProducto = oD.idProducto;
                        oDetalle.cantidad = oD.cantidad;
                        oDetalle.precioUnidad = oD.precioUnidad;
                        oDetalle.descuento = oD.descuento;
                        oDetalle.importeTotal = oD.cantidad * oD.precioUnidad - (precioConDescuento);
                        oDetalle.idOrden = o.idOrden;
                        db.DetalleOrden.Add(oDetalle);
                    }
                    db.SaveChanges();
                }
                ViewBag.idProveedor = help.GetProveedor(); //Carga proveedor
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        [Error(Roles = "Operario de Almacén")]
        [Authorize(Roles = "Operario de Almacén")]
        public ActionResult Detalles(int? id) //Detalles
        {
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

        [Error(Roles = "Administrador,Operario de Almacén")]
        [Authorize(Roles = "Administrador,Operario de Almacén")]
        public ActionResult Documento(int idOrden = 0) //Generar documento
        {
            //Buscar la orden de compra
            OrdenCompra ordenCompra = db.OrdenCompra.Find(idOrden);

            return View(ordenCompra);
        }

        [Error(Roles = "Operario de Almacén")]
        [Authorize(Roles = "Operario de Almacén")]
        public ActionResult Editar(int? id) //Editar - Menú
        {
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

        [Error(Roles = "Operario de Almacén")]
        [Authorize(Roles = "Operario de Almacén")]
        public ActionResult UpdateCabecera(int? id) //Editar cabecera - GET
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdenCompra ordenCompra = db.OrdenCompra.Find(id);
            if (ordenCompra == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProveedor = new SelectList(db.Proveedor.Where(x => x.activo == true), "idProveedor", "nombre", ordenCompra.idProveedor);
            return View(ordenCompra);
        }

        [HttpPost] //Editar cabecera - POST
        [ValidateAntiForgeryToken] 
        public ActionResult UpdateCabecera(OrdenCompra orden)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orden).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Editar", new { id = orden.idOrden });
            }
            ViewBag.idProveedor = new SelectList(db.Proveedor, "idProveedor", "nombre", orden.idProveedor); //Carga proveedor
            return View(orden);
        }

        [HttpPost] //Eliminar productos - POST
        public ActionResult DeleteProducto(int id)
        {
            Models.DetalleOrden detalleAEliminar = db.DetalleOrden.Find(id);
            db.DetalleOrden.Remove(detalleAEliminar);
            db.SaveChanges();
            db.usp_UpdateCostoNeto(detalleAEliminar.idOrden); //Procedimiento para actualizar neto
            db.SaveChanges();
            db.usp_UpdateIGV(detalleAEliminar.idOrden); //SP para actualizar IGV
            db.SaveChanges();
            db.usp_UpdateCostoTotal(detalleAEliminar.idOrden); //SP para actualizar costo total
            db.SaveChanges();
            return Json(new {success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost] // POST: OrdenCompra/Eliminar Orden/
        public ActionResult Eliminar(int id)
        {
            //Encontramos todos los registros que queremos eliminar
            var miorden = db.OrdenCompra.SingleOrDefault(a => a.idOrden == id);
            var detalleAEliminar = miorden.DetalleOrden.Where(t => t.idOrden == id);
            //Eliminarlos de un sólo golpe
            db.DetalleOrden.RemoveRange(detalleAEliminar);
            db.OrdenCompra.Remove(miorden);
            db.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        // GET: OrdenCompra/ EDITAR - Agregar productos a la orden
        [Error(Roles = "Operario de Almacén")]
        [Authorize(Roles = "Operario de Almacén")]
        public ActionResult AddProducto(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdenCompra orden = db.OrdenCompra.Find(id);
            TableOrdenCompra tablaorden = new TableOrdenCompra();

            tablaorden.idOrden = orden.idOrden;

            if (orden == null)
            {
                return HttpNotFound();
            }
            //ViewBag.idProducto = new SelectList(db.producto, "idProducto", "nombreProducto");
            return View(tablaorden);
        }

        [HttpPost]
        public ActionResult AddProducto(TableOrdenCompra model)
        {
            try
            {
                using (OLCAREntities db = new OLCAREntities())
                {
                    OrdenCompra o = new OrdenCompra();

                    foreach (var oD in model.DetalleOrden)
                    {
                        Models.DetalleOrden oDetalle = new Models.DetalleOrden();
                        decimal precioConDescuento = oD.cantidad * oD.precioUnidad * (oD.descuento / 100);
                        oDetalle.idProducto = oD.idProducto;
                        oDetalle.cantidad = oD.cantidad;
                        oDetalle.precioUnidad = oD.precioUnidad;
                        oDetalle.descuento = oD.descuento;
                        oDetalle.importeTotal = oD.cantidad * oD.precioUnidad - (precioConDescuento);
                        oDetalle.idOrden = model.idOrden;
                        db.DetalleOrden.Add(oDetalle);
                    }
                    db.SaveChanges();
                    db.usp_UpdateCostoNeto(model.idOrden);
                    db.SaveChanges();
                    db.usp_UpdateIGV(model.idOrden);
                    db.SaveChanges();
                    db.usp_UpdateCostoTotal(model.idOrden);
                    db.SaveChanges();
                }
                //ViewBag.idProducto = GetProducto(); //Carga Producto
                return RedirectToAction("Editar", new { id = model.idOrden });
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }
    }
}