using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Data;
using ProductManagement.API.Dtos;
using ProductManagement.API.Models;
using ProductManagement.API.Services;

namespace ProductManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;

        public AuthController(AppDbContext context,
                              IPasswordHasher<User> passwordHasher,
                              ITokenService tokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        // ================= REGISTER =================
        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            // ① Username / email must be unique
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                return Conflict("Username is already taken.");
            }
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return Conflict("Email is already registered.");
            }

            // ② Create the user (Role is ALWAYS "User" at registration)
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Role = "User"
            };

            // ③ Hash the password. The plain password is NEVER saved.
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            // ④ Save
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // ⑤ Return safe info only (no PasswordHash!)
            return StatusCode(201, new { user.Id, user.Username, user.Email, user.Role });
        }

        // ================= LOGIN =================
        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            // ① Find the user by username
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            // ② Check the password against the stored hash
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid username or password.");
            }

            // ③ Correct → create the JWT
            var (token, expiresAt) = _tokenService.CreateToken(user);

            // ④ Send it back
            return Ok(new AuthResponseDto
            {
                Token = token,
                Username = user.Username,
                Role = user.Role,
                ExpiresAt = expiresAt
            });
        }
    }
}