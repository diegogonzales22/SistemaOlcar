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

        private static Usuario SesionUsuario;

        public ActionResult Registrar()
        {
            if (Session["Usuario"] != null)
            {
                SesionUsuario = (Usuario)Session["Usuario"];

            }

            return View();
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