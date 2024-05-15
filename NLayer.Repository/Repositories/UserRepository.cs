using Microsoft.EntityFrameworkCore;
using NLayer.Core;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User> GetSingleUserWithCreditCardsAndAddressesAsync(int userId)
        {
            return await _context.Users.Include(x => x.Addresses).Include(x => x.CreditCards).Where(x => x.Id == userId).SingleOrDefaultAsync();
        }
    }
}
