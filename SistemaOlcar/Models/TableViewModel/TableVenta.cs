using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaOlcar.Models.TableViewModel
{
    public class TableVenta
    {
        public int idVenta { get; set; }
        public string fechaRegistro { get; set; }
        public bool estado { get; set; }
        public int idUsuario { get; set; }
        public string numeroDocumento { get; set; }
        public string nombre { get; set; }
        public decimal subTotal { get; set; }
        public decimal igv { get; set; }
        public decimal importeTotal { get; set; }
        public List<DetalleVenta> DetalleVenta { get; set; }
    }

    public class DetalleVenta
    {
        public int idDetalleVenta { get; set; }
        public int idProducto { get; set; }
        public int idVenta { get; set; }
        public int cantidad { get; set; }
        public decimal precioUnidad { get; set; }
        public decimal importeTotal { get; set; }
    }
}