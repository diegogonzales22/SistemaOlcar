using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaOlcar.Models.TableViewModel;
using SistemaOlcar.Models;
using SistemaOlcar.Helpers;
using System.Data.Entity;
using System.Net;

namespace SistemaOlcar.Controllers
{
    public class IngresoController : Controller
    {
        private OLCAREntities db = new OLCAREntities();
        private static Usuario SesionUsuario;
        static Utilitario help = new Utilitario();

        // GET: Ingreso
        [Error(Roles = "Operario de Almacén")]
        [Authorize(Roles = "Operario de Almacén")]
        public ActionResult Index()
        {
            return View();
        }

        //Listar Ingresos
        public JsonResult Listar()
        {
            List<TableIngreso> oLstIngreso = new List<TableIngreso>();

            using (OLCAREntities d = new OLCAREntities())
            {
                oLstIngreso = (from p in d.Ingreso
                               select new TableIngreso
                               {
                                   idIngreso = p.idIngreso,
                                   fechaRegistro = p.fechaRegistro.ToString(),
                                   proveedor = p.Proveedor.nombre,
                                   nroOrden = p.OrdenCompra.idOrden,
                                   nroDocumento = p.nroDocumento,
                                   costoTotal = p.costoTotal
                               }).ToList();
            }
            return Json(new { data = oLstIngreso }, JsonRequestBehavior.AllowGet);
        }

        //Obtener Ordenes aprobadas como Json
        public JsonResult OrdenesAprobadas() //Listar Ordenes
        {
            List<TableOrdenCompra> oLstOrdenes = new List<TableOrdenCompra>();
            using (OLCAREntities d = new OLCAREntities())
            {
                d.Configuration.ProxyCreationEnabled = false;
                oLstOrdenes = (from p in d.OrdenCompra
                                where p.situacion == "Aprobada"
                                select new TableOrdenCompra
                                {
                                    idOrden = p.idOrden,
                                    fechaRegistro = p.fechaRegistro.ToString(),
                                    fechaAprobacion = p.fechaAprobacion.ToString(),
                                    proveedor = p.Proveedor.nombre,
                                    costoTotal = p.costoTotal
                                }).ToList();
            }
            return Json(new { data = oLstOrdenes }, JsonRequestBehavior.AllowGet);
        }

        //Obtener Orden como Json
        public JsonResult ObtenerOrden(int idOrden)
        {
            OrdenCompra oOrden = new OrdenCompra();

            using (OLCAREntities db = new OLCAREntities())
            {
                db.Configuration.ProxyCreationEnabled = false; //Deshabilita carga diferida
                oOrden = (from p in db.OrdenCompra.Where(x => x.idOrden == idOrden)
                          select p).FirstOrDefault();
            }

            return Json(oOrden, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Registrar() //Registrar Ingreso
        {
            if (Session["Usuario"] != null)
            {
                SesionUsuario = (Usuario)Session["Usuario"];
            }

            ViewBag.idProveedor = help.GetProveedor(); //Carga proveedor
            ViewBag.idOrden = help.GetOrden(); //Carga órden de compra
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(TableIngreso model)
        {
            try
            {
                using (OLCAREntities db = new OLCAREntities())
                {
                    Ingreso o = new Ingreso();
                    o.fechaRegistro = DateTime.Now;
                    o.creadorPor = SesionUsuario.idUsuario;
                    o.tipoDocumento = model.tipoDocumento;
                    o.nroDocumento = model.nroDocumento;
                    o.fechaDocumento = model.fechaDocumento;
                    o.idOrden = model.idOrden;
                    o.idProveedor = model.idProveedor;
                    o.totalNeto = model.totalNeto;
                    o.totalIGV = model.totalIGV;
                    o.costoTotal = model.costoTotal;
                    o.observacion = model.observacion;
                    db.Ingreso.Add(o);
                    db.SaveChanges();

                    foreach (var oD in model.DetalleIngreso)
                    {
                        Models.DetalleIngreso oDetalle = new Models.DetalleIngreso();
                        decimal precioConDescuento = oD.cantidad * oD.precioUnidad * (oD.descuento / 100);
                        oDetalle.idProducto = oD.idProducto;
                        oDetalle.fecha = DateTime.Now;
                        oDetalle.cantidad = oD.cantidad;
                        oDetalle.precioUnidad = oD.precioUnidad;
                        oDetalle.descuento = oD.descuento;
                        oDetalle.importeTotal = oD.cantidad * oD.precioUnidad - (precioConDescuento);
                        oDetalle.idIngreso = o.idIngreso;
                        db.DetalleIngreso.Add(oDetalle);
                        //Procedimiento almacenado para cambiar stock
                        db.SP_IngresaStock(oDetalle.idProducto, oDetalle.cantidad);
                    }
                    db.SaveChanges();

                    //Actualiza el estado de la Orden de compra
                    var oOrden = db.OrdenCompra.Find(model.idOrden);
                    oOrden.situacion = "Surtida";
                    db.Entry(oOrden).State = EntityState.Modified;
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
        public ActionResult Detalle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingreso ingreso = db.Ingreso.Find(id);
            if (ingreso == null)
            {
                return HttpNotFound();
            }
            return View(ingreso);
        }

        [Error(Roles = "Operario de Almacén")]
        [Authorize(Roles = "Operario de Almacén")]
        public ActionResult Documento(int id = 0) //Generar documento
        {
            //Buscar la orden de compra
            Ingreso  ingreso = db.Ingreso.Find(id);

            return View(ingreso);
        }

        public ActionResult Editar(int? id) //Editar - Menú
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingreso ingreso = db.Ingreso.Find(id);
            if (ingreso == null)
            {
                return HttpNotFound();
            }
            return View(ingreso);
        }

        [Error(Roles = "Operario de Almacén")]
        [Authorize(Roles = "Operario de Almacén")]
        public ActionResult UpdateCabecera(int? id) //Editar cabecera - GET
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingreso ingreso = db.Ingreso.Find(id);
            if (ingreso == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProveedor = new SelectList(db.Proveedor.Where(x => x.activo == true), "idProveedor", "nombre", ingreso.idProveedor);
            return View(ingreso);
        }

        [HttpPost] //Editar cabecera - POST
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCabecera(Ingreso ingreso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ingreso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Editar", new { id = ingreso.idIngreso });
            }
            ViewBag.idProveedor = new SelectList(db.Proveedor, "idProveedor", "nombre", ingreso.idProveedor); //Carga proveedor
            return View(ingreso);
        }

        // GET: Ingreso/ EDITAR - Agregar productos al ingreso
        [Error(Roles = "Operario de Almacén")]
        [Authorize(Roles = "Operario de Almacén")]
        public ActionResult AddProducto(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingreso ingreso = db.Ingreso.Find(id);
            TableIngreso tablaingreso = new TableIngreso();

            tablaingreso.idIngreso = ingreso.idIngreso;

            if (tablaingreso == null)
            {
                return HttpNotFound();
            }
            return View(tablaingreso);
        }

        [HttpPost]
        public ActionResult AddProducto(TableIngreso model)
        {
            try
            {
                using (OLCAREntities db = new OLCAREntities())
                {
                    Ingreso o = new Ingreso();

                    foreach (var oD in model.DetalleIngreso)
                    {
                        Models.DetalleIngreso oDetalle = new Models.DetalleIngreso();
                        decimal precioConDescuento = oD.cantidad * oD.precioUnidad * (oD.descuento / 100);
                        oDetalle.idProducto = oD.idProducto;
                        oDetalle.fecha = DateTime.Now;
                        oDetalle.cantidad = oD.cantidad;
                        oDetalle.precioUnidad = oD.precioUnidad;
                        oDetalle.descuento = oD.descuento;
                        oDetalle.importeTotal = oD.cantidad * oD.precioUnidad - (precioConDescuento);
                        oDetalle.idIngreso = model.idIngreso;
                        db.DetalleIngreso.Add(oDetalle);
                        //Procedimiento almacenado para cambiar stock
                        db.SP_IngresaStock(oDetalle.idProducto, oDetalle.cantidad);
                    }
                    db.SaveChanges();
                    db.usp_ActualizarCostoNeto(model.idIngreso);
                    db.SaveChanges();
                    db.usp_ActualizarIGV(model.idIngreso);
                    db.SaveChanges();
                    db.usp_ActualizarCostoTotal(model.idIngreso);
                    db.SaveChanges();
                }
                //ViewBag.idProducto = GetProducto(); //Carga Producto
                return RedirectToAction("Editar", new { id = model.idIngreso });
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        [HttpPost] //Eliminar productos - POST
        public ActionResult DeleteProducto(int id)
        {
            Models.DetalleIngreso detalleAEliminar = db.DetalleIngreso.Find(id);
            var myid = detalleAEliminar.idIngreso;
            var idProd = detalleAEliminar.idProducto;

            db.DetalleIngreso.Remove(detalleAEliminar);
            db.SaveChanges();
            db.usp_ActualizarCostoNeto(myid); //Procedimiento para actualizar neto
            db.SaveChanges();
            db.usp_ActualizarIGV(myid); //SP para actualizar IGV
            db.SaveChanges();
            db.usp_ActualizarCostoTotal(myid); //SP para actualizar costo total
            db.SaveChanges();
            //Disminuye el stock
            db.SP_RestaStock(idProd, detalleAEliminar.cantidad);
            db.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}