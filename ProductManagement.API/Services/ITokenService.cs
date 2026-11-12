using ProductManagement.API.Models;

namespace ProductManagement.API.Services
{
    public interface ITokenService
    {
        (string Token, DateTime ExpiresAt) CreateToken(User user);
    }
}