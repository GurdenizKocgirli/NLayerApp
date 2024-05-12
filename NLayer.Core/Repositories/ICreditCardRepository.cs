using NLayer.Core.Models;

namespace NLayer.Core.Repositories
{
    public interface ICreditCardRepository : IGenericRepository<CreditCard>
    {
        Task<CreditCard> GetSingleUserByIdWithCreditCards(int creditCardId);
    }
}
