using ProductManagement.API.Models;

namespace ProductManagement.API.Services
{
    // The CONTRACT: what a supplier service can do
    public interface ISupplierService
    {
        Task<List<Supplier>> GetAllAsync();
        Task<Supplier?> GetByIdAsync(int id);
        Task<List<Supplier>> GetActiveAsync();
    }
}