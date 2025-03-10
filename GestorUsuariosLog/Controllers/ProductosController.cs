using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestorUsuariosLog.Data;
using GestorUsuariosLog.Models;
using Microsoft.AspNetCore.Authorization;

namespace GestorUsuariosLog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await _context.Productos.ToListAsync();
        }

        // GET: api/productos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound(new { message = "Producto no encontrado" });
            return producto;
        }

        // POST: api/productos
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            producto.Id = 0; // Se genera automáticamente
            _context.Productos.Add(producto);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "Error al guardar producto.", error = ex.Message });
            }
            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
        }

        // PUT: api/productos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.Id)
                return BadRequest(new { message = "El ID del producto no coincide." });

            _context.Entry(producto).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Productos.Any(p => p.Id == id))
                    return NotFound(new { message = "Producto no encontrado" });
                else
                    throw;
            }
            return NoContent();
        }

        // DELETE: api/productos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound(new { message = "Producto no encontrado" });
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/productos/agregaciones
        [HttpGet("Estadistica")]
        public async Task<IActionResult> GetAgregaciones()
        {
            var productos = await _context.Productos.ToListAsync();
            if (!productos.Any())
                return NotFound(new { message = "No hay productos registrados" });

            var productoPrecioMax = productos.OrderByDescending(p => p.Precio).First();
            var productoPrecioMin = productos.OrderBy(p => p.Precio).First();
            var sumaPrecios = productos.Sum(p => p.Precio);
            var promedioPrecios = productos.Average(p => p.Precio);

            var resultado = new
            {
                ProductoConPrecioMaximo = productoPrecioMax,
                ProductoConPrecioMinimo = productoPrecioMin,
                SumaTotalPrecios = sumaPrecios,
                PrecioPromedio = promedioPrecios
            };

            return Ok(resultado);
        }

        // GET: api/productos/total
        [HttpGet("TotalProductos")]
        public async Task<IActionResult> GetTotalProductos()
        {
            var total = await _context.Productos.CountAsync();
            return Ok(new { TotalProductosRegistrados = total });
        }

        // GET: api/productos/categoria/{categoriaId}
        [HttpGet("categoria/{categoriaId}")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductosPorCategoria(int categoriaId)
        {
            var productos = await _context.Productos.Where(p => p.IdCategoria == categoriaId).ToListAsync();
            if (!productos.Any())
                return NotFound(new { message = "No se encontraron productos para esta categoría" });
            return Ok(productos);
        }

        // GET: api/productos/proveedor/{proveedorId}
        [HttpGet("proveedor/{proveedorId}")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductosPorProveedor(int proveedorId)
        {
            var productos = await _context.Productos.Where(p => p.IdProveedor == proveedorId).ToListAsync();
            if (!productos.Any())
                return NotFound(new { message = "No se encontraron productos para este proveedor" });
            return Ok(productos);
        }
    }
}
