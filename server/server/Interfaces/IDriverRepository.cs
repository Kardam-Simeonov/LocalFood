using server.Dto;
using server.Models;

namespace server.Interfaces
{
    public interface IDriverRepository
    {
        Task<Driver> GetDriverById(int driverId);
        Task<Driver> GetDriverByEmail(string email);
        Task AddDriver(Driver driver);
        Task UpdateDriver(Driver driver, DriverUpdateDto driverDto);
        Task RemoveDriver(Driver driver);
    }
}
