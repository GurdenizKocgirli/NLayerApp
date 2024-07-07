using NLayer.Core.Models;

namespace NLayer.Core.Repositories
{
    public interface IAddressRepository : IGenericRepository<Address> //Address nesnesi için IAddressRepository'de özel metodlar tanımlanır
    {
        Task<List<Address>> GetAddressesWithUsers(); //Asenkron olarak List<Address> döndürülür
    }
}
