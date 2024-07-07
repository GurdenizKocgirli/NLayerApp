using NLayer.Core.Models;

namespace NLayer.Core.Repositories
{
    public interface ICreditCardRepository : IGenericRepository<CreditCard> //genel crud işlemlerini tekrardan yazmamak için IGenericRepository'den miras alınır ve CreditCard türünde nesne için bu işlem yapılır
    {
        Task<List<CreditCard>> GetCreditCardsWithUsers(); //Asenkron olarak List<CreditCard> döndürülür
    }
}
