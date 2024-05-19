using server.Dto;
using server.Models;

namespace server.Interfaces
{
    public interface IVendorRepository
    {
        Task<Vendor> GetVendorById(int vendorId);
        Task<Vendor> GetVendorByEmail(string email);
        Task AddVendor(Vendor vendor);
        Task UpdateVendor(Vendor vendor, VendorUpdateDto vendorDto);
        Task RemoveVendor(Vendor vendor);
    }
}
