using NLayer.Core.Models;

namespace NLayer.Core.Repositories
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        Task<List<Address>> GetAddressesWithUsers();
    }
}
