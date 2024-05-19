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
        private readonly IDriverRepository _driverRepository;

        public AuthController(IConfiguration configuration, IVendorRepository vendorRepository, IDriverRepository driverRepository)
        {
            this._configuration = configuration;
            _vendorRepository = vendorRepository;
            _driverRepository = driverRepository;
        }

        [HttpPost("vendor/register")]
        public async Task<ActionResult<Vendor>> RegisterVendor(VendorRegisterDto request)
        {
            // Check if the user already exists
            if (await _vendorRepository.GetVendorByEmail(request.Email) != null)
            {
                return Conflict("User already exists.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // Create a new user entity
            var newUser = new Vendor
            {
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
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
            await _vendorRepository.AddVendor(newUser);

            return Ok(newUser);
        }

        [HttpPost("vendor/login")]
        public async Task<ActionResult<string>> LoginVendor(UserLoginDto request)
        {
            // Find the user in the database
            var user = await _vendorRepository.GetVendorByEmail(request.Email);

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
        [HttpPost("driver/register")]
        public async Task<ActionResult<Driver>> RegisterDriver(DriverRegisterDto request)
        {
            // Check if the user already exists
            if (await _driverRepository.GetDriverByEmail(request.Email) != null)
            {
                return Conflict("User already exists.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // Create a new user entity
            var newUser = new Driver
            {
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
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
            await _driverRepository.AddDriver(newUser);

            return Ok(newUser);
        }

        [HttpPost("driver/login")]
        public async Task<ActionResult<string>> LoginDriver(UserLoginDto request)
        {
            // Find the user in the database
            var user = await _driverRepository.GetDriverByEmail(request.Email);

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
                return NotFound("Vendor not found");

            await _vendorRepository.UpdateVendor(vendor, vendorDto);

            return Ok();
        }

        [HttpDelete("vendor/{id}")]
        public async Task<IActionResult> RemoveVendor(int id)
        {
            var vendor = await _vendorRepository.GetVendorById(id);

            if (vendor == null)
                return NotFound("Vendor not found");

            await _vendorRepository.RemoveVendor(vendor);

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
                PhoneNumber = vendor.PhoneNumber,
                Address = vendor.Address,
                Latitude = vendor.Latitude,
                Longitude = vendor.Longitude,
                Image = vendor.Image
            };

            return Ok(vendorDto);
        }

        [HttpPut("driver/{id}")]
        public async Task<IActionResult> UpdateDriver(int id, DriverUpdateDto driverDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var driver = await _driverRepository.GetDriverById(id);

            if (driver == null)
                return NotFound("Driver not found");

            await _driverRepository.UpdateDriver(driver, driverDto);

            return Ok();
        }

        [HttpDelete("driver/{id}")]
        public async Task<IActionResult> RemoveDriver(int id)
        {
            var driver = await _driverRepository.GetDriverById(id);

            if (driver == null)
                return NotFound("Driver not found");

            await _driverRepository.RemoveDriver(driver);

            return Ok();
        }

        [HttpGet("driver/{id}")]
        public async Task<IActionResult> GetDriverById(int id)
        {
            var driver = await _driverRepository.GetDriverById(id);

            if (driver == null)
                return NotFound("Driver not found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var driverDto = new DriverEditDto
            {
                Id = driver.Id,
                Name = driver.Name,
                Email = driver.Email,
                PhoneNumber = driver.PhoneNumber,
                Image = driver.Image
            };

            return Ok(driverDto);
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

        private string CreateToken(Driver user)
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
