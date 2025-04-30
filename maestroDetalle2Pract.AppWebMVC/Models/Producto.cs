using System;
using System.Collections.Generic;

namespace maestroDetalle2Pract.AppWebMVC.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int? Stock { get; set; }

    public virtual ICollection<DetallesVentas> DetallesVentas { get; set; } = new List<DetallesVentas>();
}
