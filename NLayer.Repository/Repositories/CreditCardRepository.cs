using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Core.Repositories;

namespace NLayer.Repository.Repositories
{
    public class CreditCardRepository : GenericRepository<CreditCard>, ICreditCardRepository
    {
        public CreditCardRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<CreditCard>> GetCreditCardsWithUsers()
        {
            return await _context.CreditCards.Include(x => x.User).ToListAsync();
        }
    }
}
