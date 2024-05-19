using Azure.Core;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dto;
using server.Interfaces;
using server.Models;

namespace server.Repository
{
    public class VendorRepository : IVendorRepository
    {
        private readonly DataContext _context;
        public VendorRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Vendor> GetVendorById(int vendorId)
        {
            return await _context.Vendors.FirstOrDefaultAsync(s => s.Id == vendorId);
        }
        public async Task<Vendor> GetVendorByEmail(string email)
        {
            var user = await _context.Vendors.SingleOrDefaultAsync(u => u.Email == email);

            return user;
        }
        public async Task AddVendor(Vendor vendor)
        {
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateVendor(Vendor vendor, VendorUpdateDto vendorDto)
        {
            vendor.Name = vendorDto.Name;
            vendor.Email = vendorDto.Email;
            vendor.Address = vendorDto.Address;
            vendor.PhoneNumber = vendorDto.PhoneNumber;
            vendor.Longitude = vendorDto.Longitude;
            vendor.Latitude = vendorDto.Latitude;

            if (vendorDto.Image != null && vendorDto.Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await vendorDto.Image.CopyToAsync(memoryStream);
                    vendor.Image = memoryStream.ToArray();
                }
            }

            await _context.SaveChangesAsync();
        }
        public async Task RemoveVendor(Vendor vendor)
        {
            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();
        }
    }
}
