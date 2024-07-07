using NLayer.Core.Models;

namespace NLayer.Core.Repositories
{
    public interface IUserRepository : IGenericRepository<User> //User nesnesinin genel crud işlemleri için IGenericRepositoryden miras alınır ve tekrardan bu işlemleri IUserRepository içerisinde yazmaya gerek kalmaz 
    {
        Task<User> GetSingleUserWithCreditCardsAndAddressesAsync(int userId); //belirli bir user id'si alınır ve geriye asenkron olarak bir User döndürülür
    }
}
