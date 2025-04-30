using System;
using System.Collections.Generic;

namespace maestroDetalle2Pract.AppWebMVC.Models;

public partial class DetallesVentas
{
    public int Id { get; set; }

    public int? VentaId { get; set; }

    public int? ProductoId { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal? Subtotal { get; set; }

    public virtual Producto? Producto { get; set; }

    public virtual Venta? Venta { get; set; }
}
