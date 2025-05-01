using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using maestroDetalle2Pract.AppWebMVC.Models;

namespace maestroDetalle2Pract.AppWebMVC.Controllers
{
    public class VentasController : Controller
    {
        private readonly MaestroDetallePract2DbContext _context;

        public VentasController(MaestroDetallePract2DbContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index(Venta venta, int topRegistro=10)
        {
            var query = _context.Ventas.AsQueryable();
            if (!string.IsNullOrWhiteSpace(venta.NombreCliente))
                query = query.Where(s => s.NombreCliente.Contains(venta.NombreCliente));
            if (venta.FechaVenta.HasValue && venta.FechaVenta.Value != DateTime.MinValue)
            {
                query = query.Where(s => s.FechaVenta.HasValue && s.FechaVenta.Value.Date == venta.FechaVenta.Value.Date);
            }
            if (topRegistro > 0)
                query = query.Take(topRegistro);
            return View(await query.ToListAsync());
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                 .Include(s => s.DetallesVentas)//se agrego un join para obtener todos los detalles de venta
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venta == null)
            {
                return NotFound();
            }

            ViewBag.Productos = _context.Productos;
            return View();
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            return View(new Venta());
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Correlativo,FechaVenta,Total,NombreCliente, Detallesventas")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venta);
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            return View(venta);
        }

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Correlativo,FechaVenta,Total,NombreCliente")] Venta venta)
        {
            if (id != venta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(venta);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta != null)
            {
                _context.Ventas.Remove(venta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.Id == id);
        }
        public IActionResult AddItemDetallesVenta(Venta venta)
        {
            if (venta.DetallesVentas == null)
                venta.DetallesVentas = new List<DetallesVentas>();
            venta.DetallesVentas.Add(new DetallesVentas
            {
                Cantidad = 1,
                NumItem = venta.DetallesVentas.Count + 1,
                ProductoId = 0,
                Subtotal = 0m
            });
            var productos = new SelectList(_context.Productos, "Id", "Nombre");
            ViewBag.Productos = _context.Productos;
            foreach (var item in venta.DetallesVentas)
            {
                item.Subtotal = item.Cantidad * item.PrecioUnitario;
            }
            return PartialView("_detalleventa", venta.DetallesVentas);
        }
        public IActionResult DeleteItemDetallesVenta(int id, Venta venta)
        {
            int num = id;
            if (venta.DetallesVentas.Count == 0)
                venta.DetallesVentas = new List<DetallesVentas>();
            var detalleDel = venta.DetallesVentas.FirstOrDefault(s => s.NumItem == num);
            if (detalleDel != null)
            {
                venta.DetallesVentas.Remove(detalleDel);
                int numItemNew = 1;
                foreach (var item in venta.DetallesVentas)
                {
                    item.NumItem = numItemNew;
                    item.Subtotal = item.Cantidad * item.PrecioUnitario;
                    numItemNew++;
                }
            }
            ViewBag.Productos = _context.Productos;
            return PartialView("_detalleventa", venta.DetallesVentas);
        }
        public IActionResult UpdateItemDetallesVenta(Venta venta)
        {
            ViewBag.Productos = _context.Productos;
            foreach (var item in venta.DetallesVentas)
            {
                item.Subtotal = item.Cantidad * item.PrecioUnitario;
            }
            return PartialView("_detalleventa", venta.DetallesVentas);
        }
    }
}
