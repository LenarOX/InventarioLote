//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InventarioLote
{
    using System;
    using System.Collections.Generic;
    
    public partial class Movimientos
    {
        public int MovimientoId { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public int ProductoLoteId { get; set; }
        public int cantidad { get; set; }
        public decimal Total { get; set; }
    
        public virtual ProductoLote ProductoLote { get; set; }
    }
}
