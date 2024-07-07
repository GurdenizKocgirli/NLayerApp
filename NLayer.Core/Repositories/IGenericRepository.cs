using System.Linq.Expressions;

namespace NLayer.Core.Repositories
{
    public interface IGenericRepository<T> where T : class // T adında bir type parameter alır ve bu type bir class olmak zorundadır
    {
        Task<T> GetByIdAsync(int id); //Belirli bir id ile eşleşen bir nesneyi asenkron olarak alır ve T türünde bir nesne döndürür
        IQueryable<T> GetAll(); //db'deki tüm T türündeki nesneleri döndürür, IQueryable geri dönüş tipi LINQ sorguları için kullanılabilmesini sağlar
        IQueryable<T> Where(Expression<Func<T, bool>> expression); //belirli bir şartı sağlayan T türündeki nesneler bool olarak döndürülür ve IQueryable olarak döndürülmesi db'de sorgu atılmasını sağlar
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression); //belirli bir şartı sağlayan herhangi bir T türünde nesne olup olmadığını asenkron olarak kontrol eder, şartı sağlayan en az bir nesne varsa true yoksa false döner
        Task AddAsync(T entity); //belirli bir T türündeki nesneyi asenkron olarak db'ye ekler
        Task AddRangeAsync(IEnumerable<T> entities); //birden fazla T türündeki nesneyi asenkron olarak db'ye ekler. IEnumerable<T> eklenecek nesnelerin bir koleksiyonunu belirtir
        void Update(T entity); //T türünde mevcut bir nesneyi günceller ve void bu metodun geriye herhangi bir değer döndürmeyeceğini belirtir yani sadece işlem yapılır
        void Remove(T entity); //T türünde db'de yer alan mevcut bir nesneyi db'den siler ve geriye herhangi bir değer dönmez 
        void RemoveRange(IEnumerable<T> entities); //T türündeki belirli bir nesne grubunu db'den siler ve geriye bir değer dönmez burada IEnumerable silinecek olan nesnelerin koleksiyonunu belirtir
    }
}
