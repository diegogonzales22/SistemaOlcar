//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SistemaOlcar.Models
{
    using System;
    
    public partial class SP_Login_Result
    {
        public int idUsuario { get; set; }
        public string usuario { get; set; }
        public byte[] contraseña { get; set; }
        public int idRol { get; set; }
        public bool estado { get; set; }
    }
}
