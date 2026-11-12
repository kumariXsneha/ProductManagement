using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Data;
using ProductManagement.API.Models;
using AutoMapper;
using ProductManagement.API.Dtos;

namespace ProductManagement.API.Controllers
{
    [ApiController] // turns on the API features
    [Route("api/[controller]")]          // URL = api/products
    public class ProductsController : ControllerBase // Gives helpers like Ok(), NotFound(), BadRequest()
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;                       // NEW


        // DI gives us AppDbContext (same flow you debugged)
        public ProductsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;                                   // NEW
        }

        // automapping
        // GET: api/products/dto
        [HttpGet("dto")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsDto()
        {
            var products = await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)          // ⚠️ required, or Category is null → CategoryName is null
                .ToListAsync();

            var dtos = _mapper.Map<List<ProductDto>>(products);
            return Ok(dtos);
        }

        // ============ READ ALL ============
        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);                                   // 200 OK
        }

        // ============ READ ONE ============
        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound($"Product with Id {id} not found."); // 404

            return Ok(product);                                    // 200 OK
        }

        // ============ CREATE ============
        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.Products.Add(product);        // 1. mark as "Added" (no SQL yet)
            await _context.SaveChangesAsync();     // 2. NOW runs INSERT, Id is filled in

            // 201 Created + URL of the new product
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // ============ UPDATE ============
        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest("Id in URL and body must match."); // 400

            var existing = await _context.Products.FindAsync(id);
            if (existing == null)
                return NotFound($"Product with Id {id} not found."); // 404

            // Copy new values onto the tracked entity
            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.StockQuantity = product.StockQuantity;
            existing.IsActive = product.IsActive;
            // CreatedDate is NOT changed on purpose

            await _context.SaveChangesAsync();     // runs UPDATE (only changed columns)
            return NoContent();                    // 204
        }

        // ============ DELETE ============
        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound($"Product with Id {id} not found."); // 404

            _context.Products.Remove(product);     // mark as "Deleted" (no SQL yet)
            await _context.SaveChangesAsync();     // NOW runs DELETE
            return NoContent();                    // 204
        }
    }
}