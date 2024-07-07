namespace NLayer.Core.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category> //IGenericRepository'den Category türündeki nesneler için özel metodlar ICategoryRepository'de tanımlanır
    {
        Task<Category> GetSingleCategoryByIdWithProductsAsync(int categoryId); //belirli bir category id'si alınır ve Category asenkron olarak döndürülür
    }
}
