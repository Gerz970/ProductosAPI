using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductosAPI.Models;


[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public ProductosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Productos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
    {
        return await _context.Productos.Include(p => p.IdCategoriaNavigation).ToListAsync();
    }

    // GET: api/Productos/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Producto>> GetProducto(int id)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "El ID debe ser mayor que cero." });

        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
            return NotFound(new { mensaje = $"Producto con ID {id} no encontrado." });

        return Ok(producto);
    }

    // Productos por categoria
    [HttpGet("ProductosCategoria/{idCategoria:int}")]
    public async Task<ActionResult<IEnumerable<Producto>>> GetProductosByCategoria(int idCategoria)
    {
        if (idCategoria <= 0)
            return BadRequest(new { mensaje = "El ID de categoría debe ser mayor que cero." });
        var productos = await _context.Productos
            .Where(p => p.IdCategoria == idCategoria && p.Activo == true)
            .ToListAsync();
        if (productos.Count == 0)
            return NotFound(new { mensaje = $"No se encontraron productos para la categoría con ID {idCategoria}." });
        return Ok(productos);
    }

    // consultar categorias

    [HttpGet("Categorias")]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
    {
        return await _context.Categorias.Include(c => c.Productos)
            .Where(c => c.Activo == true)
            .ToListAsync();
    }


}
