using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Core.Repositories;

namespace NLayer.Repository.Repositories
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Address>> GetAddressesWithUsers()
        {
            return await _context.Addresses.Include(x => x.User).ToListAsync(); 
        }
    }
}
