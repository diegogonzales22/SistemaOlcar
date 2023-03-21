using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaOlcar.Models;
using System.Net;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using SistemaOlcar.Models.TableViewModel;

namespace SistemaOlcar.Controllers
{
    public class VentaController : Controller
    {
        OLCAREntities db = new OLCAREntities();
        private static Usuario SesionUsuario;

        public ActionResult ListarVentas()
        {
            return View();
        }

        public JsonResult MisVentas() //Lista ventas realizadas
        {
            List<TableVenta> oLstOrden = new List<TableVenta>();
            using (OLCAREntities d = new OLCAREntities())
            {
                //d.Configuration.ProxyCreationEnabled = false; 
                oLstOrden = (from p in d.Venta
                             select new TableVenta
                             {
                                 idVenta = p.idVenta,
                                 fechaRegistro = p.fechaRegistro.ToString(),
                                 numeroDocumento = p.numeroDocumento,
                                 importeTotal = p.importeTotal,
                                 estado = p.estado
                             }).ToList();
            }
            return Json(new { data = oLstOrden }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Registrar() //Registrar una venta
        {
            if (Session["Usuario"] != null)
            {
                SesionUsuario = (Usuario)Session["Usuario"];

            }

            return View();
        }

        [HttpPost]
        public ActionResult Registrar(TableVenta model)
        {
            try
            {
                Venta o = new Venta();
                using (OLCAREntities db = new OLCAREntities())
                {
                    o.fechaRegistro = DateTime.Now;
                    o.estado = true;
                    o.idUsuario = SesionUsuario.idUsuario;
                    o.numeroDocumento = model.numeroDocumento;
                    o.nombre = model.nombre;
                    o.subTotal = model.subTotal;
                    o.igv = model.igv;
                    o.importeTotal = model.importeTotal;
                    db.Venta.Add(o);
                    db.SaveChanges();

                    foreach (var oD in model.DetalleVenta)
                    {
                        Models.DetalleVenta oDetalle = new Models.DetalleVenta();
                        oDetalle.fechaRegistro = DateTime.Now;
                        oDetalle.idProducto = oD.idProducto;
                        oDetalle.cantidad = oD.cantidad;
                        oDetalle.precioUnidad = oD.precioUnidad;
                        oDetalle.importeTotal = oD.cantidad * oD.precioUnidad;
                        oDetalle.idVenta = o.idVenta;
                        db.DetalleVenta.Add(oDetalle);
                        //Procedimiento almacenado para cambiar stock
                        db.SP_RestaStock(oDetalle.idProducto, oDetalle.cantidad);
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Detalles", new { id = o.idVenta });
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        public ActionResult Detalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venta venta = db.Venta.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }

        public ActionResult Ticket(int id = 0)
        {
            //Buscar la venta
            Venta venta = db.Venta.Find(id);
            return View(venta);
        }

        public JsonResult Obtener(string dni) //Obtener DNI de API
        {
            Cliente oVenta = new Cliente();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"https://api.apis.net.pe/v1/dni?numero=" + dni);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                oVenta = JsonConvert.DeserializeObject<Cliente>(json);
            }

            return Json(oVenta, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarProductos()
        {
            return View();
        }

        public JsonResult ListarProd() //Listar Productos
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
                                    precioVenta = (decimal)p.precioVenta,
                                    codigoEAN = p.codigoEAN,
                                    marca = p.MarcaProducto.nombre,
                                    stock = p.stock,
                                    unidad = p.unidadMedida,
                                    ubicacion = p.Ubicacion.descripcion
                                }).ToList();
            }
            return Json(new { data = oLstProducto }, JsonRequestBehavior.AllowGet);
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
                                    unidadMedida = p.unidadMedida,
                                    codigoEAN = p.codigoEAN,
                                    marca = p.MarcaProducto.nombre,
                                    stock = p.stock,
                                    precioVenta = (decimal)p.precioVenta
                                }).ToList();
            }
            return Json(new { data = oLstProducto }, JsonRequestBehavior.AllowGet);
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

    }

    //------CLIENTE-----
    public class Cliente
    {
        public string nombre { get; set; }
        public string numeroDocumento { get; set; }
    }
}