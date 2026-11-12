using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Data;
using ProductManagement.API.Models;
using ProductManagement.API.Services;
using ProductManagement.API.Dtos;
using AutoMapper;

namespace ProductManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly AppDbContext _context;               // still used by the methods not moved yet
        private readonly ISupplierService _supplierService;   // NEW: the service layer
        private readonly IMapper _mapper;                            // NEW

        public SuppliersController(AppDbContext context, ISupplierService supplierService, IMapper mapper)
        {
            _context = context;
            _supplierService = supplierService;
            _mapper = mapper;                                        // NEW

        }
        // automapper

        // GET: api/suppliers/dto-manual
        // DEMO: mapping by hand (we'll replace this with AutoMapper in A3)
        [HttpGet("dto-manual")]
        public async Task<Act
            ionResult<IEnumerable<SupplierDto>>> GetSuppliersManual()
        {
            var suppliers = await _supplierService.GetAllAsync();

            /*var dtos = suppliers.Select(s => new SupplierDto
            {
                Id = s.Id,
                Name = s.Name,
                ContactPerson = s.ContactPerson,
                Email = s.Email,
                Phone = s.Phone,
                City = s.City,
                Rating = s.Rating,
                IsActive = s.IsActive
            }).ToList();

            return Ok(dtos);
        } */

            var dtos = _mapper.Map<List<SupplierDto>>(suppliers);   // 🖨️ ONE line

            return Ok(dtos);
        }




        // ======================= GET (via SERVICE) =======================

        // GET: api/suppliers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        {
            var suppliers = await _supplierService.GetAllAsync();
            return Ok(suppliers);
        }

        /* GET: api/suppliers/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);

            if (supplier == null)
            {
                return NotFound($"Supplier with Id {id} not found.");
            }

            return Ok(supplier);
        } */
        //Get Supplier by ID by mapping 
        // GET: api/suppliers/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<SupplierDto>> GetSupplier(int id)       // ← SupplierDto now
        {
            var supplier = await _supplierService.GetByIdAsync(id);

            if (supplier == null)
            {
                return NotFound($"Supplier with Id {id} not found.");
            }

            var dto = _mapper.Map<SupplierDto>(supplier);                       // entity → ID card
            return Ok(dto);
        }
        //mapping 
        // GET: api/suppliers/cards
        [HttpGet("cards")]
        public async Task<ActionResult<IEnumerable<SupplierCardDto>>> GetSupplierCards()
        {
            var suppliers = await _supplierService.GetAllAsync();
            var cards = _mapper.Map<List<SupplierCardDto>>(suppliers);
            return Ok(cards);
        }

        // GET: api/suppliers/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetActive()
        {
            var activeSuppliers = await _supplierService.GetActiveAsync();
            return Ok(activeSuppliers);
        }

        // ======================= GET (practice queries) =======================

        // GET: api/suppliers/city/Pune
        [HttpGet("city/{city}")]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetByCityName(string city)
        {
            var suppliers = await _context.Suppliers
                .AsNoTracking()
                .Where(s => s.City == city)
                .ToListAsync();

            return Ok(suppliers);
        }

        
        // GET: api/suppliers/rating?min=4
        [HttpGet("rating")]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetByMinRating([FromQuery] int min)
        {
            var topRated = await _context.Suppliers
                .AsNoTracking()
                .Where(s => s.Rating >= min)
                .ToListAsync();

            return Ok(topRated);
        }

        // ======================= POST =======================

        /* POST: api/suppliers  (with duplicate email check)
        [HttpPost]
        public async Task<ActionResult<Supplier>> CreateSupplier(Supplier supp)
        {
            var existEmail = await _context.Suppliers.AnyAsync(s => s.Email == supp.Email);
            if (existEmail)
            {
                return Conflict($"A supplier with email {supp.Email} already exists.");
            }

            _context.Suppliers.Add(supp);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSupplier), new { id = supp.Id }, supp);
        } */
        // post by mapping 
          // POST: api/suppliers
[HttpPost]
public async Task<ActionResult<SupplierDto>> CreateSupplier(CreateSupplierDto createDto)
{
    // 1. Business rule: no duplicate emails (same as before)
    var existEmail = await _context.Suppliers.AnyAsync(s => s.Email == createDto.Email);
    if (existEmail)
    {
        return Conflict($"A supplier with email {createDto.Email} already exists.");
    }

    // 2. FORM → FILE:  CreateSupplierDto → Supplier entity
    var supplier = _mapper.Map<Supplier>(createDto);

    // 3. Save to the database (the DB creates the Id)
    _context.Suppliers.Add(supplier);
    await _context.SaveChangesAsync();

    // 4. FILE → ID CARD:  Supplier entity → SupplierDto
    var resultDto = _mapper.Map<SupplierDto>(supplier);

    // 5. 201 Created + location header + the DTO
    return CreatedAtAction(nameof(GetSupplier), new { id = resultDto.Id }, resultDto);
} 

        // ======================= PUT =======================

        // PUT: api/suppliers/2
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateSupplier(int id, Supplier supp)
        {
            if (id != supp.Id)
            {
                return BadRequest("Id in URL and body must match.");
            }

            var existing = await _context.Suppliers.FindAsync(id);
            if (existing == null)
            {
                return NotFound($"Supplier with Id {id} not found.");
            }

            existing.Name = supp.Name;
            existing.ContactPerson = supp.ContactPerson;
            existing.Email = supp.Email;
            existing.Phone = supp.Phone;
            existing.City = supp.City;
            existing.Rating = supp.Rating;
            existing.IsActive = supp.IsActive;
            // Id and CreatedDate are NOT copied on purpose

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ======================= PATCH =======================

        // PATCH: api/suppliers/2
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchSupplier(int id, SupplierPatchRequest patch)
        {
            var existing = await _context.Suppliers.FindAsync(id);
            if (existing == null)
            {
                return NotFound($"Supplier with Id {id} not found.");
            }

            if (patch.Name != null) existing.Name = patch.Name;
            if (patch.ContactPerson != null) existing.ContactPerson = patch.ContactPerson;
            if (patch.Email != null) existing.Email = patch.Email;
            if (patch.Phone != null) existing.Phone = patch.Phone;
            if (patch.City != null) existing.City = patch.City;
            if (patch.Rating.HasValue) existing.Rating = patch.Rating.Value;
            if (patch.IsActive.HasValue) existing.IsActive = patch.IsActive.Value;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ======================= DELETE =======================

        // DELETE: api/suppliers/9
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound($"Supplier with Id {id} not found.");
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ======================= LINQ lessons =======================

        // ORDER BY: api/suppliers/ranked
        [HttpGet("ranked")]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetRanked()
        {
            var ranked = await _context.Suppliers
                .AsNoTracking()
                .OrderByDescending(s => s.Rating)
                .ThenBy(s => s.Name)
                .ToListAsync();

            return Ok(ranked);
        }

        // SELECT columns: api/suppliers/contacts
        [HttpGet("contacts")]
        public async Task<IActionResult> GetContacts()
        {
            var contacts = await _context.Suppliers
                .AsNoTracking()
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .Select(s => new { s.Name, s.Email, s.Phone, s.City })
                .ToListAsync();

            return Ok(contacts);
        }

        // COUNT / AVG / MAX / MIN: api/suppliers/stats
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var total = await _context.Suppliers.CountAsync();
            var active = await _context.Suppliers.CountAsync(s => s.IsActive);
            var average = await _context.Suppliers.AverageAsync(s => s.Rating);
            var highest = await _context.Suppliers.MaxAsync(s => s.Rating);
            var lowest = await _context.Suppliers.MinAsync(s => s.Rating);

            return Ok(new
            {
                totalSuppliers = total,
                activeSuppliers = active,
                averageRating = Math.Round(average, 2),
                highestRating = highest,
                lowestRating = lowest
            });
        }

        // TOP N: api/suppliers/top/3
        [HttpGet("top/{n:int}")]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetTop(int n)
        {
            if (n <= 0)
            {
                return BadRequest("N must be greater than 0.");
            }

            var topSuppliers = await _context.Suppliers
                .AsNoTracking()
                .OrderByDescending(s => s.Rating)
                .ThenBy(s => s.Name)
                .Take(n)
                .ToListAsync();

            return Ok(topSuppliers);
        }

        // PAGING: api/suppliers/paged?page=2&pageSize=3
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 3)
        {
            if (page < 1)
            {
                return BadRequest("Page must be 1 or more.");
            }
            if (pageSize < 1 || pageSize > 50)
            {
                return BadRequest("PageSize must be between 1 and 50.");
            }

            var totalCount = await _context.Suppliers.CountAsync();

            var items = await _context.Suppliers
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return Ok(new { page, pageSize, totalCount, totalPages, items });
        }

        // SEARCH (dynamic filters): api/suppliers/search?name=a&city=Pune&minRating=4&isActive=true&sortBy=rating_desc
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? name,
            [FromQuery] string? city,
            [FromQuery] int? minRating,
            [FromQuery] bool? isActive,
            [FromQuery] string? sortBy,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            if (page < 1)
            {
                return BadRequest("Page must be 1 or more.");
            }
            if (pageSize < 1 || pageSize > 50)
            {
                return BadRequest("PageSize must be between 1 and 50.");
            }

            IQueryable<Supplier> query = _context.Suppliers.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(s => s.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(s => s.City == city);

            if (minRating.HasValue)
                query = query.Where(s => s.Rating >= minRating.Value);

            if (isActive.HasValue)
                query = query.Where(s => s.IsActive == isActive.Value);

            query = sortBy switch
            {
                "name_desc" => query.OrderByDescending(s => s.Name),
                "rating" => query.OrderBy(s => s.Rating),
                "rating_desc" => query.OrderByDescending(s => s.Rating),
                _ => query.OrderBy(s => s.Name)
            };

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return Ok(new { page, pageSize, totalCount, totalPages, items });
        }

        // GROUP BY: api/suppliers/by-city
        [HttpGet("by-city")]
        public async Task<IActionResult> GetByCity()
        {
            var report = await _context.Suppliers
                .GroupBy(s => s.City)
                .Select(g => new
                {
                    City = g.Key,
                    SupplierCount = g.Count(),
                    ActiveCount = g.Count(s => s.IsActive),
                    AverageRating = g.Average(s => s.Rating)
                })
                .OrderByDescending(x => x.SupplierCount)
                .ThenBy(x => x.City)
                .ToListAsync();

            return Ok(report);
        }
    }
}