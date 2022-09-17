using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaOlcar.Models;

namespace SistemaOlcar.Helpers
{
    public class Utilitario
    {
        //Obtiene Proveedores
        public IEnumerable<SelectListItem> GetProveedor()
        {
            var data = new OLCAREntities();
            return data.Proveedor.Where(x=>x.activo == true).Select(x => new SelectListItem
            {
                Text = x.nombre,
                Value = x.idProveedor.ToString()
            }).ToList();
        }

        //Obtiene Productos
        public IEnumerable<SelectListItem> GetProducto()
        {
            var data = new OLCAREntities();
            return data.Producto.Where(x=>x.estado == true).Select(x => new SelectListItem
            {
                Text = x.nombre,
                Value = x.idProducto.ToString()
            }).ToList();
        }

    }
}