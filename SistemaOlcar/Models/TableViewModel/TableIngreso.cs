using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaOlcar.Models.TableViewModel
{
    public class TableIngreso
    {
        public int idIngreso { get; set; }
        public string fechaRegistro { get; set; }
        public string proveedor { get; set; }
        public string tipoDocumento { get; set; }
        public string nroDocumento { get; set; }
        public decimal costoTotal { get; set; }
        public int nroOrden { get; set; }

        public DateTime fechaDocumento { get; set; }
        public string observacion { get; set; }
        public int idOrden { get; set; }
        public int idProveedor { get; set; }
        public decimal totalNeto { get; set; }
        public decimal totalIGV { get; set; }
        public List<DetalleIngreso> DetalleIngreso { get; set; }

        //Para el historial de compras
        public int idProducto { get; set; }
        public string producto { get; set; }
        public int cantidad { get; set; }
        public decimal importeTotal { get; set; }
        public decimal descuento { get; set; }
        public decimal costoSinIGV { get; set; }
        public decimal costoConIGV { get; set; }
    }

    //--------------------------------
    public class DetalleIngreso
    {
        public int idProducto { get; set; }
        public int cantidad { get; set; }
        public decimal precioUnidad { get; set; }
        public decimal descuento { get; set; }
    }
}