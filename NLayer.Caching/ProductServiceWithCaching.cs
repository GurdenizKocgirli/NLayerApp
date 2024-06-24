using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.Exceptions;
using System.Linq.Expressions; //gerekli namespace'lerin kullanıma alınması.

namespace NLayer.Caching
{
    public class ProductServiceWithCaching : IProductService //ürünlere daha hızlı bir şekilde ulaşabilmek için(önbellek) caching sınıfı kullanılması
    {
        private const string CacheProductKey = "productsCache"; //CacheProductKey önbellekte saklanan ürün verilerini tanımlamak, erişmek için bir key olarak kullanılır
        private readonly IMapper _mapper; //IMapper interface'inden türetilmiş bir instance. Mapping işlemlerini yapmayı sağlar.
        private readonly IMemoryCache _memoryCache; //IMemoryCache interface'inden türetilmiş bir instance, sık erişilen verilerin bellekte tutularak daha hızlı erişilmesini sağlar.
        private readonly IProductRepository _repository; //readonly olarak tanımlanması _repository değişkeninin sadece constructor'da tanımlanabilmesini ifade eder. Sınıfın geri kalanında değiştirilemez.
        private readonly IUnitOfWork _unitOfWork; //Veri tabanı işlemlerinin gerçekleşmesini sağlar ve burada UnitOfWork deseninin kullanılması ya bütün işlemler başarılı olacak ya da hiçbir işlem gerçekleşmeyecek mentalitesiyle ilerler.

        public ProductServiceWithCaching(IUnitOfWork unitOfWork, IProductRepository repository, IMemoryCache memoryCache, IMapper mapper) //Constructor içerisinde belirli bir cache anahtarı altında 'CacheProductKey' ürün verilerinin önbelleğe alınmasını sağlar.
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _memoryCache = memoryCache;
            _mapper = mapper;

            if (!_memoryCache.TryGetValue(CacheProductKey, out _)) //_memoryCache.TryGetValue metodu ile 'CacheProductKey' anahtarının önbellekte olup olmadığı kontrol edilir.
            {//Eğer anahtar önbellekte varsa metod true döner ve değeri out parametresine atar. _ kullanıldı yani değeri önemsenmiyor.
                _memoryCache.Set(CacheProductKey, _repository.GetProductsWithCategory().Result);
            }
            //Eğer 'CacheProductKey' anahtarı önbellekte yoksa _memoryCache.TryGetValue metodu false döner ve _repository.GetProductsWithCategory().Result kullanılarak ürünler ve kategoriler veritabanından alınır ve bu veri 'CacheProductKey' anahtarı altında önbelleğe eklenir.

        }

        public async Task<Product> AddAsync(Product entity) //entity parametresi db'ye eklenen yeni bir ürünü eklemek için kullanılır.
        {
            await _repository.AddAsync(entity); //db'ye asenkron olarak yeni ürünün eklenmesini sağlar.
            await _unitOfWork.CommitAsync(); //db'ye yansıtılır ve kalıcı hale getirilir.
            await CacheAllProductsAsync(); // _memoryCache'deki tüm verileri yeniden önbelleğe almayı sağlar.
            return entity; //metodun result'u olarak yeni eklenen ürün döndürülür.
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities) //Birden fazla entity'nin yani IEnumerable<Product> tipindeki entities parametresinin db'ye eklenmesini sağlar.
        {
            await _repository.AddRangeAsync(entities); //entities koleksiyonundaki tüm entity'lerin db'ye toplu bir şekilde eklenmesini sağlar.
            await _unitOfWork.CommitAsync(); 
            await CacheAllProductsAsync();
            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Product, bool>> expression) //Belirli bir koşulu sağlayan bir entity'nin db'de olup olmadığını kontrol eden bir metoddur.
        {
            //metodun dönüş tipi Task<bool>'dur. Ya var ya yok. Task=asenkron işlem.
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync() //_memoryCache üzerinden daha önce önbelleğe alınmış olan tüm ürünleri 'IEnumerable<Product>' döndürür. Önbellekteki verileri döndürür yani tekrardan db'ye sorgu atmakla uğraşmayız. Neden yaparız? Performans arttırmak için.
        {
            var products = _memoryCache.Get<IEnumerable<Product>>(CacheProductKey); //'IEnumerable<Product>' türünde Get metodu ile CacheProductKey anahtarına sahip product'lar alınır.
            return Task.FromResult(products);   //Task.FromResult metodu ile products değişkeni Task<IEnumerable<Product>> türüne dönüştürülür ve döndürülür.
        }

        public Task<Product> GetByIdAsync(int id)
        {
            var product = _memoryCache.Get<List<Product>>(CacheProductKey).FirstOrDefault(x => x.Id == id); //_memoryCache nesnesi üzerinden CacheProductKey anahtarına sahip olan List<Product> koleksiyonu alınır. FirstOrDefault kullanılarak bu koleksiyondaki belirli bir id'ye sahip olan ilk ürün bulunur.

            if (product == null) //eğer belirtilen id'ye sahip ürün önbellekte bulunamazsa NotFoundExcepiton hatası fırlatılır.
            {
                throw new NotFoundExcepiton($"{typeof(Product).Name}({id}) not found");
            }

            return Task.FromResult(product); //product property'si Task.FromResult metodu ile birlikte Task<Product> türüne dönüştürülür ve döndürülür.
        }

        public Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory()
        {
            var products = _memoryCache.Get<IEnumerable<Product>>(CacheProductKey);

            var productsWithCategoryDto = _mapper.Map<List<ProductWithCategoryDto>>(products);

            return Task.FromResult(CustomResponseDto<List<ProductWithCategoryDto>>.Success(200,productsWithCategoryDto));
        }

        public async Task RemoveAsync(Product entity)
        {
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entities) //IEnumerable<Product> türünde entities parametresinin yani ürünlerin silinmesi metodu.
        {
            _repository.RemoveRange(entities); 
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync(); //_memoryCache üzerinde bulunan verilerin önbellekte güncellenmesini sağlar.
        }

        public async Task UpdateAsync(Product entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync(); //_memoryCache üzerinde bulunan verilerin güncellenmesini sağlar yani önbelleği günceller.
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression) //_memoryCache üzerinde önbelleğe alınmış verilerin IQueryable<Product> türünde döndürülmesi. Amaç önbelleğe alınmış veriler üzerinden sorgulama yapmaktır.
        {
            return _memoryCache.Get<List<Product>>(CacheProductKey).Where(expression.Compile()).AsQueryable();
        }

        public async Task CacheAllProductsAsync()
        {
            _memoryCache.Set(CacheProductKey, await _repository.GetAll().ToListAsync()); //asenkron olarak veri çekilmesi ve bu verilerin _memoryCache üzerinden önbelleğe alınmasını sağlar.
        }
    }
}
