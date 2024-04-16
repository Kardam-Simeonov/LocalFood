using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Repository
{
    public class VendorRepository
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
    }
}
