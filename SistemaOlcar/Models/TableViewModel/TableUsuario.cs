using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaOlcar.Models.TableViewModel
{
    public class TableUsuario
    {
        public int idUsuario { get; set; }
        public string usuario { get; set; }
        public string contraseña { get; set; }
        public string rol { get; set; }
        public bool estado { get; set; }
    }
}