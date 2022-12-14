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

namespace SistemaOlcar.Controllers
{
    public class OrdenCompraController : Controller
    {
        private OLCAREntities db = new OLCAREntities();
        private static Usuario SesionUsuario;
        static Utilitario help = new Utilitario();

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
                                    fechaRegistro = p.fechaRegistro.ToString(),
                                    proveedor = p.Proveedor.nombre,
                                    situacion = p.situacion,
                                    costoTotal = p.costoTotal
                                }).ToList();
            }
            return Json(new { data = oLstOrden }, JsonRequestBehavior.AllowGet);
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

        public ActionResult Documento(int idOrden = 0) //Generar documento
        {
            //Buscar la orden de compra
            OrdenCompra ordenCompra = db.OrdenCompra.Find(idOrden);

            return View(ordenCompra);
        }

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
            return Json(new {success = true, message = "Se eliminó con éxito"}, JsonRequestBehavior.AllowGet);
        }
    }
}