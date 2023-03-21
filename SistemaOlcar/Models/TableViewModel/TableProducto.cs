using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaOlcar.Models.TableViewModel
{
    public class TableProducto
    {
        public int idProducto { get; set; }
        public string nombre { get; set; }
        public decimal precioVenta { get; set; }
        public string codigoEAN { get; set; }
        public string unidad { get; set; }
        public string marca { get; set; }
        public long stock { get; set; }
        public bool estado { get; set; }
        public int stockMinimo { get; set; }
        public int stockMaximo { get; set; }

        public string ubicacion { get; set; }
        public string unidadMedida { get; set; }

    }
}