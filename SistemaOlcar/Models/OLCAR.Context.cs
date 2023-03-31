﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class OLCAREntities : DbContext
    {
        public OLCAREntities()
            : base("name=OLCAREntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<MarcaProducto> MarcaProducto { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<Proveedor> Proveedor { get; set; }
        public virtual DbSet<Ubicacion> Ubicacion { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<DetalleOrden> DetalleOrden { get; set; }
        public virtual DbSet<OrdenCompra> OrdenCompra { get; set; }
        public virtual DbSet<DetalleIngreso> DetalleIngreso { get; set; }
        public virtual DbSet<Ingreso> Ingreso { get; set; }
        public virtual DbSet<DetalleVenta> DetalleVenta { get; set; }
        public virtual DbSet<Venta> Venta { get; set; }
    
        public virtual int usp_UpdateCostoNeto(Nullable<int> idOrden)
        {
            var idOrdenParameter = idOrden.HasValue ?
                new ObjectParameter("idOrden", idOrden) :
                new ObjectParameter("idOrden", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_UpdateCostoNeto", idOrdenParameter);
        }
    
        public virtual int usp_UpdateCostoTotal(Nullable<int> idOrden)
        {
            var idOrdenParameter = idOrden.HasValue ?
                new ObjectParameter("idOrden", idOrden) :
                new ObjectParameter("idOrden", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_UpdateCostoTotal", idOrdenParameter);
        }
    
        public virtual int usp_UpdateIGV(Nullable<int> idOrden)
        {
            var idOrdenParameter = idOrden.HasValue ?
                new ObjectParameter("idOrden", idOrden) :
                new ObjectParameter("idOrden", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_UpdateIGV", idOrdenParameter);
        }
    
        public virtual int SP_IngresaStock(Nullable<int> idProducto, Nullable<int> cantidad)
        {
            var idProductoParameter = idProducto.HasValue ?
                new ObjectParameter("idProducto", idProducto) :
                new ObjectParameter("idProducto", typeof(int));
    
            var cantidadParameter = cantidad.HasValue ?
                new ObjectParameter("cantidad", cantidad) :
                new ObjectParameter("cantidad", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_IngresaStock", idProductoParameter, cantidadParameter);
        }
    
        public virtual int usp_ActualizarCostoNeto(Nullable<int> idIngreso)
        {
            var idIngresoParameter = idIngreso.HasValue ?
                new ObjectParameter("idIngreso", idIngreso) :
                new ObjectParameter("idIngreso", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_ActualizarCostoNeto", idIngresoParameter);
        }
    
        public virtual int usp_ActualizarCostoTotal(Nullable<int> idIngreso)
        {
            var idIngresoParameter = idIngreso.HasValue ?
                new ObjectParameter("idIngreso", idIngreso) :
                new ObjectParameter("idIngreso", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_ActualizarCostoTotal", idIngresoParameter);
        }
    
        public virtual int usp_ActualizarIGV(Nullable<int> idIngreso)
        {
            var idIngresoParameter = idIngreso.HasValue ?
                new ObjectParameter("idIngreso", idIngreso) :
                new ObjectParameter("idIngreso", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_ActualizarIGV", idIngresoParameter);
        }
    
        public virtual int SP_RestaStock(Nullable<int> idProducto, Nullable<int> cantidad)
        {
            var idProductoParameter = idProducto.HasValue ?
                new ObjectParameter("idProducto", idProducto) :
                new ObjectParameter("idProducto", typeof(int));
    
            var cantidadParameter = cantidad.HasValue ?
                new ObjectParameter("cantidad", cantidad) :
                new ObjectParameter("cantidad", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_RestaStock", idProductoParameter, cantidadParameter);
        }
    
        public virtual ObjectResult<SP_RetornaVentas_Result> SP_RetornaVentas()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_RetornaVentas_Result>("SP_RetornaVentas");
        }
    
        public virtual ObjectResult<Nullable<decimal>> SP_GananciaDia()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("SP_GananciaDia");
        }
    
        public virtual ObjectResult<Nullable<int>> SP_CantidadProductosDia()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SP_CantidadProductosDia");
        }
    
        public virtual ObjectResult<Nullable<int>> SP_CantidadVentasDia()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SP_CantidadVentasDia");
        }
    
        public virtual ObjectResult<SP_RetornaGananciasDiarias_Result> SP_RetornaGananciasDiarias()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_RetornaGananciasDiarias_Result>("SP_RetornaGananciasDiarias");
        }
    
        public virtual ObjectResult<SP_RetornaTopProductos_Result> SP_RetornaTopProductos()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_RetornaTopProductos_Result>("SP_RetornaTopProductos");
        }
    
        public virtual ObjectResult<Nullable<long>> SP_CantidadStockTotal()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<long>>("SP_CantidadStockTotal");
        }
    }
}
