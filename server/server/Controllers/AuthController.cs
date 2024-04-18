using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using server.Data;
using server.Dto;
using server.Interfaces;
using server.Models;
using server.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IVendorRepository _vendorRepository;
        private readonly DataContext _context;

        public AuthController(DataContext context, IConfiguration configuration, IVendorRepository vendorRepository)
        {
            this._configuration = configuration;
            this._context = context;
            _vendorRepository = vendorRepository;
        }

        [HttpPost("vendor/register")]
        public async Task<ActionResult<Vendor>> Register(UserRegisterDto request)
        {
            // Check if the user already exists
            if (await _context.Vendors.AnyAsync(u => u.Email == request.Email))
            {
                return Conflict("User already exists.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // Create a new user entity
            var newUser = new Vendor
            {
                Name = request.Name,
                Email = request.Email,
                Address = request.Address,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            if (request.Image != null && request.Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await request.Image.CopyToAsync(memoryStream);
                    newUser.Image = memoryStream.ToArray();
                }
            }

            // Add the user to the database
            _context.Vendors.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(newUser);
        }

        [HttpPost("vendor/login")]
        public async Task<ActionResult<string>> Login(UserLoginDto request)
        {
            // Find the user in the database
            var user = await _context.Vendors.SingleOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Verify password
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            // Create and return JWT token
            string token = CreateToken(user);
            return Ok(new { token = token, userId = user.Id });
        }

        [HttpPut("vendor/{id}")]
        public async Task<IActionResult> UpdateVendor(int id, VendorUpdateDto vendorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vendor = await _vendorRepository.GetVendorById(id);

            if (vendor == null)
                return NotFound("Product not found");

            await _vendorRepository.UpdateVendorById(vendor, vendorDto);

            return Ok();
        }

        [HttpGet("vendor/{id}")]
        public async Task<IActionResult> GetVendorById(int id)
        {
            var vendor = await _vendorRepository.GetVendorById(id);

            if (vendor == null)
                return NotFound("Vendor not found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vendorDto = new VendorEditDto
            {
                Id = vendor.Id,
                Name = vendor.Name,
                Email = vendor.Email,
                Address = vendor.Address,
                Latitude = vendor.Latitude,
                Longitude = vendor.Longitude,
                Image = vendor.Image
            };

            return Ok(vendorDto);
        }

        private string CreateToken(Vendor user) 
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Name)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt)) 
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
