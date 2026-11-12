using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Data;
using ProductManagement.API.Models;

namespace ProductManagement.API.Services
{
    // The IMPLEMENTATION: how it's actually done
    public class SupplierService : ISupplierService
    {
        private readonly AppDbContext _context;

        public SupplierService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task<List<Supplier>> GetActiveAsync()
        {
            return await _context.Suppliers
                .AsNoTracking()
                .Where(s => s.IsActive)
                .ToListAsync();
        }
    }
}