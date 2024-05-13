using NLayer.Core.Models;

namespace NLayer.Core.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetSingleUserWithCreditCardsAndAddressesAsync(int userId);
    }
}
