using server.Dto;
using server.Models;

namespace server.Interfaces
{
    public interface IVendorRepository
    {
        Task<Vendor> GetVendorById(int vendorId);
        Task UpdateVendorById(Vendor vendor, VendorUpdateDto vendorDto);
        Task RemoveVendor(Vendor vendor);
    }
}
