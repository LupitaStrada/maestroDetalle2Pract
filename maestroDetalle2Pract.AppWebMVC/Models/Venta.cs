using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace maestroDetalle2Pract.AppWebMVC.Models;

public partial class Venta
{
    public int Id { get; set; }

    public string Correlativo { get; set; } = null!;
    [Display(Name = "Fecha de venta")]
    public DateTime? FechaVenta { get; set; }

    public decimal? Total { get; set; }
    [Display(Name = "Nombre de cliente")]
    public string? NombreCliente { get; set; }

    public virtual ICollection<DetallesVentas> DetallesVentas { get; set; } = new List<DetallesVentas>();
}
