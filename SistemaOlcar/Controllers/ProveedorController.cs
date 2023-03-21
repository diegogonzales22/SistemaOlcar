using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using SistemaOlcar.Models;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace SistemaOlcar.Controllers
{
    public class ProveedorController : Controller
    {
        public ActionResult Index() //Inicio
        {
            return View();
        }

        public JsonResult Listar() //Lista proveedores
        {
            List<Proveedor> oLstProveedor = new List<Proveedor>();
            using (OLCAREntities d = new OLCAREntities())
            {
                d.Configuration.ProxyCreationEnabled = false;
                oLstProveedor = (from p in d.Proveedor
                                 select p).ToList();
            }
            return Json(new { data = oLstProveedor }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Registrar() //GET: Proveedor
        {
            return View();
        }

        public JsonResult Obtener(string ruc) //Obtener RUC de API
        {
            Proveedor oProveedor = new Proveedor();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"https://api.apis.net.pe/v1/ruc?numero=" + ruc);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                oProveedor = JsonConvert.DeserializeObject<Proveedor>(json);
            }

            return Json(oProveedor, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Registrar(Proveedor proveedor)
        {
            OLCAREntities bd = new OLCAREntities();
            bool existe = bd.Proveedor.Any(x => x.numeroDocumento == proveedor.numeroDocumento);

            try
            {
                if (ModelState.IsValid)
                {
                    if (existe == true)
                    {
                        ViewBag.error = "El proveedor que pretende ingresar ya existe";
                        return View(proveedor);
                    }
                    else
                    {
                        using (OLCAREntities db = new OLCAREntities())
                        {
                            db.Proveedor.Add(proveedor);
                            db.SaveChanges();
                            TempData["exito"] = "El proveedor fue registrado con éxito";
                        }
                        return RedirectToAction("Registrar", "Proveedor");
                    }
                }
            }
            catch (Exception ex)
            { 
                throw new Exception(ex.Message); 
            }
            return View(proveedor);
        }

        public JsonResult Details(int idProveedor) //Detalles de proveedor
        {
            Proveedor oProveedor = new Proveedor();
            using (OLCAREntities d = new OLCAREntities())
            {
                d.Configuration.ProxyCreationEnabled = false; //Deshabilita carga diferida
                oProveedor = (from p in d.Proveedor
                             where p.idProveedor == idProveedor
                             select p).FirstOrDefault();
            }
            return Json(oProveedor, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Editar(int? id)
        {
            using (OLCAREntities db = new OLCAREntities())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Proveedor proveedor = db.Proveedor.Find(id);
                if (proveedor == null)
                {
                    return HttpNotFound();
                }
                return View(proveedor);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Proveedor proveedor)
        {
            OLCAREntities bd = new OLCAREntities();
            bool existe = bd.Proveedor.Any(x => x.numeroDocumento == proveedor.numeroDocumento);

            try
            {
                if (ModelState.IsValid)
                {
                    using (OLCAREntities db = new OLCAREntities())
                    {
                        db.Entry(proveedor).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["exito"] = "El proveedor fue modificado con éxito";
                    }
                    return RedirectToAction("Editar", new { id = proveedor.idProveedor });
                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(proveedor);
        }

        public ActionResult ListaProvAdmin()
        {
            return View();
        }

    }
}
