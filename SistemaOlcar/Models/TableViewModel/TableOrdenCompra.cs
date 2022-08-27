using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaOlcar.Models.TableViewModel
{
    public class TableOrdenCompra
    {
        public int idOrden { get; set; }
        public DateTime fechaRegistro { get; set; }
        public string proveedor { get; set; }
        public decimal costoTotal { get; set; }
        public string situacion { get; set; }
    }
}