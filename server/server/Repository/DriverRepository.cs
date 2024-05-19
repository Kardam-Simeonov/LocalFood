using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dto;
using server.Interfaces;
using server.Models;

namespace server.Repository
{
    public class DriverRepository : IDriverRepository
    {
        private readonly DataContext _context;
        public DriverRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Driver> GetDriverById(int driverId)
        {
            return await _context.Drivers.FirstOrDefaultAsync(d => d.Id == driverId);
        }
        public async Task<Driver> GetDriverByEmail(string email)
        {
            var user = await _context.Drivers.SingleOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task AddDriver(Driver driver)
        {
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateDriver(Driver driver, DriverUpdateDto driverDto)
        {
            driver.Name = driverDto.Name;
            driver.Email = driverDto.Email;
            driver.PhoneNumber = driverDto.PhoneNumber;

            if (driverDto.Image != null && driverDto.Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await driverDto.Image.CopyToAsync(memoryStream);
                    driver.Image = memoryStream.ToArray();
                }
            }

            await _context.SaveChangesAsync();
        }
        public async Task RemoveDriver(Driver driver)
        {
            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();
        }
    }
}
