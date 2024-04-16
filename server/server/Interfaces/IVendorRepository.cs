using server.Models;

namespace server.Interfaces
{
    public interface IVendorRepository
    {
        Task<Vendor> GetVendorById(int vendorId);
    }
}
