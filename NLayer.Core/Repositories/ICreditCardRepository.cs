using NLayer.Core.Models;

namespace NLayer.Core.Repositories
{
    public interface ICreditCardRepository : IGenericRepository<CreditCard>
    {
        Task<List<CreditCard>> GetCreditCardsWithUsers();
    }
}
