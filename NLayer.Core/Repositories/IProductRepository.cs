namespace NLayer.Core.Repositories
{
    public interface IProductRepository : IGenericRepository<Product> //IProductRepository, Product türündeki nesneler için özel metodlar oluşturmak için kullanılan bir arayüzdür. Genel crud işlemleri IGenericRepository üzerinden Product nesnesi için yapılır
    {
        Task<List<Product>> GetProductsWithCategory(); //Task bu metodun asenkron olarak çalışacağını belirtir ve bu metod çalışmayı tamamladığında geriye bir List<Product> dönecektir
    }
}
