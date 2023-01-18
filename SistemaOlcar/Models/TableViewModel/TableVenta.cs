using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaOlcar.Models.TableViewModel
{
    public class TableVenta
    {
        public decimal subTotal { get; set; }
        public decimal igv { get; set; }
        public decimal importeTotal { get; set; }
    }
}