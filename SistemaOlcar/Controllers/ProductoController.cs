using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaOlcar.Models;
using System.IO;
using System.Net;
using SistemaOlcar.Models.TableViewModel;
using System.Data.Entity;

namespace SistemaOlcar.Controllers
{
    public class ProductoController : Controller
    {
        OLCAREntities db = new OLCAREntities();
        // GET: Producto
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Listar() //Listar Productos
        {
            List<TableProducto> oLstProducto = new List<TableProducto>();
            using (OLCAREntities d = new OLCAREntities())
            {
                d.Configuration.ProxyCreationEnabled = false;
                oLstProducto = (from p in d.Producto
                                select new TableProducto
                                {
                                    idProducto = p.idProducto,
                                    nombre = p.nombre,
                                    precioVenta = (decimal)p.precioVenta,
                                    codigoEAN = p.codigoEAN,
                                    marca = p.MarcaProducto.nombre,
                                    estado = p.estado,
                                    stock = p.stock
                                }).ToList();
            }
            return Json(new { data = oLstProducto }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Registrar() //GET: Producto
        {
            ViewBag.idMarca = new SelectList(db.MarcaProducto.Where(x => x.estado == true).OrderBy(x => x.nombre), "idMarca", "nombre");
            ViewBag.idUbicacion = new SelectList(db.Ubicacion.Where(x => x.estado == true).OrderBy(x => x.descripcion), "idUbicacion", "descripcion");
            return View();
        }

        // POST: Producto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar([Bind(Include = "idProducto,nombre,estado, idMarca, categoria, idUbicacion, unidadMedida, codigoEAN, " +
            "costo, costoSinIGV, precioVenta, ganancia, stockMinimo, stockMaximo")] Producto producto, HttpPostedFileBase FileBase)
        {

            OLCAREntities bd = new OLCAREntities();
            bool existe = bd.Producto.Any(x => x.codigoEAN == producto.codigoEAN);

            if (ModelState.IsValid)
            {
                if (FileBase != null && FileBase.ContentLength > 0)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(FileBase.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(FileBase.ContentLength);
                    }
                    producto.imagen = imageData;
                }
                if (existe == true)
                {
                    ViewBag.error = "El código EAN que pretende ingresar ya existe";
                    ViewBag.idMarca = new SelectList(db.MarcaProducto.Where(x => x.estado == true).OrderBy(x => x.nombre), "idMarca", "nombre");
                    ViewBag.idUbicacion = new SelectList(db.Ubicacion.Where(x => x.estado == true).OrderBy(x => x.descripcion), "idUbicacion", "descripcion");
                    return View(producto);
                }
                else
                {
                    using (OLCAREntities db = new OLCAREntities())
                    {
                        db.Configuration.ProxyCreationEnabled = false;
                        producto.fechaCreacion = DateTime.Now;
                        producto.stock = 0;
                        db.Producto.Add(producto);
                        db.SaveChanges();
                        TempData["exito"] = "El producto fue registrado con éxito";
                    }
                    return RedirectToAction("Registrar", "Producto");
                }
            }

            return View(producto);
        }

        //Detalles de Producto
        public ActionResult Detalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        //GET: PRODUCTO
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.idUbicacion = new SelectList(db.Ubicacion.Where(x => x.estado == true).OrderBy(x => x.descripcion), "idUbicacion", "descripcion", producto.idUbicacion);
            ViewBag.idMarca = new SelectList(db.MarcaProducto.Where(x => x.estado == true).OrderBy(x => x.nombre), "idMarca", "nombre", producto.idMarca);
            return View(producto);
        }

        // POST: PRODUCTO / EDITAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "idProducto, fechaCreacion, nombre,estado, idMarca, categoria, idUbicacion, unidadMedida, codigoEAN, " +
            "costo, costoSinIGV, precioVenta, ganancia, stockMinimo, stockMaximo, stock")] Producto producto, HttpPostedFileBase imagen)
        {
            if (imagen != null && imagen.ContentLength > 0)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(imagen.InputStream))
                {
                    imageData = binaryReader.ReadBytes(imagen.ContentLength);
                }
                producto.imagen = imageData;
            }
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                TempData["exito"] = "El producto fue modificado con éxito";
                return RedirectToAction("Editar", new { id = producto.idProducto });
            }
            ViewBag.idUbicacion = new SelectList(db.Ubicacion.Where(x => x.estado == true).OrderBy(x => x.descripcion), "idUbicacion", "descripcion", producto.idUbicacion);
            ViewBag.idMarca = new SelectList(db.MarcaProducto.Where(x => x.estado == true).OrderBy(x => x.nombre), "idMarca", "nombre", producto.idMarca);
            return View(producto);
        }

        //GET: EDITAR IMAGEN
        public ActionResult UpdateImagen(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: EDITAR IMAGEN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateImagen(Producto producto, HttpPostedFileBase FileBase)
        {
            if (ModelState.IsValid)
            {
                if (FileBase != null && FileBase.ContentLength > 0)
                {
                    byte[] foto = null;
                    using (var binaryReader = new BinaryReader(FileBase.InputStream))
                    {
                        foto = binaryReader.ReadBytes(FileBase.ContentLength);
                    }
                    producto.imagen = foto;
                }

                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Edit", new { id = ingreso.idIngreso });
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        public ActionResult ConvertirImagen(int id) //Convertir foto
        {
            var foto = db.Producto.Where(x => x.idProducto == id).FirstOrDefault();
            return File(foto.imagen, "image/jpeg");
        }

        public ActionResult ListaProdAdmin()
        {
            return View();
        }
    }

}