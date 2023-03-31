using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using SistemaOlcar.Models;
using SistemaOlcar.Models.TableViewModel;

namespace SistemaOlcar.Controllers
{
    public class MarcaProductoController : Controller
    {
        // GET: MarcaProducto
        [Error(Roles = "Operario de Almacén")]
        [Authorize(Roles = "Operario de Almacén")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Listar() //Listar Marca de Productos
        {
            List<TableMarcaProducto> oLstMarca = new List<TableMarcaProducto>();
            using (OLCAREntities d = new OLCAREntities())
            {
                d.Configuration.ProxyCreationEnabled = true;
                oLstMarca = (from p in d.MarcaProducto 
                             select new TableMarcaProducto
                             {
                                 idMarca = p.idMarca,
                                 nombre = p.nombre,
                                 estado = p.estado
                             }
                             ).ToList();
            }
            return Json(new { data = oLstMarca }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Obtener(int idmarca) //Obtiene para editar
        {
            MarcaProducto oMarca = new MarcaProducto();

            using (OLCAREntities db = new OLCAREntities())
            {
                db.Configuration.ProxyCreationEnabled = false; //Deshabilita carga diferida
                oMarca = (from p in db.MarcaProducto.Where(x => x.idMarca == idmarca)
                          select p).FirstOrDefault();
            }

            return Json(oMarca, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(MarcaProducto oMarca) //Registra o edita una marca de producto
        {
            bool respuesta = true;
            try
            {

                if (oMarca.idMarca == 0)
                {
                    using (OLCAREntities db = new OLCAREntities())
                    {
                        db.MarcaProducto.Add(oMarca);
                        db.SaveChanges();
                    }
                }
                else
                {
                    using (OLCAREntities db = new OLCAREntities())
                    {
                        MarcaProducto tempMarca = (from p in db.MarcaProducto
                                                    where p.idMarca == oMarca.idMarca
                                                    select p).FirstOrDefault();

                        tempMarca.nombre = oMarca.nombre;
                        tempMarca.estado = oMarca.estado;

                        db.SaveChanges();
                        
                    }
                }
            }
            catch
            {
                respuesta = false;

            }

            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);

        }
    }
}