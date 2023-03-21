using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaOlcar.Models.TableViewModel
{
    public class TableOrdenCompra
    {
        public int idOrden { get; set; }
        public string fechaRegistro { get; set; }
        public string proveedor { get; set; }
        public decimal costoTotal { get; set; }
        public string situacion { get; set; }

        public int idUsuario { get; set; }
        public int idProveedor { get; set; }
        public decimal totalNeto { get; set; }
        public decimal totalIGV { get; set; }
        public DateTime fechaCaducidad { get; set; }
        public string fechaVencimiento { get; set; }
        public string observacion { get; set; }
        public string fechaAprobacion { get; set; }
        public List<DetalleOrden> DetalleOrden { get; set; }

    }

    //--------------------------------
    public class DetalleOrden
    {
        public int idProducto { get; set; }
        public int cantidad { get; set; }
        public decimal precioUnidad { get; set; }
        public decimal descuento { get; set; }
    }
}