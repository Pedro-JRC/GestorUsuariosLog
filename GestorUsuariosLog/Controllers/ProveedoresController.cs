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
    public class ProveedoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProveedoresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/proveedores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proveedor>>> GetProveedores()
        {
            return await _context.Proveedores.ToListAsync();
        }

        // GET: api/proveedores/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Proveedor>> GetProveedor(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
                return NotFound(new { message = "Proveedor no encontrado" });
            return proveedor;
        }

        // POST: api/proveedores
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Proveedor>> PostProveedor(Proveedor proveedor)
        {
            proveedor.Id = 0;
            _context.Proveedores.Add(proveedor);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "Error al guardar proveedor.", error = ex.Message });
            }
            return CreatedAtAction(nameof(GetProveedor), new { id = proveedor.Id }, proveedor);
        }

        // PUT: api/proveedores/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProveedor(int id, Proveedor proveedor)
        {
            if (id != proveedor.Id)
                return BadRequest(new { message = "El ID del proveedor no coincide." });

            _context.Entry(proveedor).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Proveedores.Any(p => p.Id == id))
                    return NotFound(new { message = "Proveedor no encontrado" });
                else
                    throw;
            }
            return NoContent();
        }

        // DELETE: api/proveedores/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProveedor(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
                return NotFound(new { message = "Proveedor no encontrado" });
            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
