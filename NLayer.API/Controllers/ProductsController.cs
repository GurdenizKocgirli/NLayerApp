using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    public class ProductsController : CustomBaseController //CustomBaseController sınıfı base bir class olduğu için içerisinde zaten Route mekanizması vardır bu yüzden ProductsController class'ında tekrardan bir route tanımlamamıza gerek yoktur. Route gelen isteklerin hangi Controller sınıfına ve action methoduna yönlendireceğine karar veren bir özelliktir
    {
        private readonly IMapper _mapper; //Mapping işlemlerinin yapılabilmesi için IMapper türünde bağımlı bir _mapper değişkeninin oluşturulması
        private readonly IProductService _service; //Product ile ilgili business işlemlerinin yapılabilmesi için IProductService türünde bir _service değişkeninin tanımlanması

        public ProductsController(IMapper mapper, IProductService productService) //Bağımlılıkların azaltılması için IMapper türünde bir mapper parametresi ve IProductService türünde bir productService parametresi tanımladık ve bağımlılığı dışarıdan vermiş olduk
        {
            _mapper = mapper;
            _service = productService;
        }

        /// GET api/products/GetProductsWithCategory --> bu kısım bizim endpoint'imizdir yani client'ın istek attığı url aslında budur. endpointlerin temiz bir şekilde kodlanması da best practice açısından önemlidir
        [HttpGet("[action]")] //Client bana Kategorilerle birlikte bütün ürünleri göster isteği atar ve action metoduna düşer
        public async Task<IActionResult> GetProductsWithCategory() //await _service.GetProductsWithCategory metodu asenkron olarak ürünlerle birlikte kategorilerini getirir
        {//projemizde asenkron işlemler yaptık çünkü bir işlemin tamamlanmasını beklerken aynı zamanda diğer işlemlerin de yapılmasını sağlamak performansı arttırır burada da async ile bu metodun asenkron olarak çalışacağını belirtmiş oluyoruz
            return CreateActionResult(await _service.GetProductsWithCategory()); //CreateActionResult ile getirilen ürünler isteği atan client'a düzenlenip http yanıtı ile gösterilir ve await ile birlikte bu işlemin yapılması beklenirken diğer işlemlerin yapılabilmesi sağlanır
        }

        /// GET api/products
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var products = await _service.GetAllAsync(); //await asenkron metodun tamamlanmasını bekler ve sonucu products değişkenine atar
            var productsDtos = _mapper.Map<List<ProductDto>>(products.ToList()); // Map<List<ProductDto>>(products.ToList()) products listesindeki her bir ürünü ProductDto nesnesine dönüştürerek productsDtos listesine atar
            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, productsDtos)); //CreateActionResult, CustomResponseDto nesnesini alır ve bir IActionResult döndürür ve bu return ile HTTP istemcisine yanıt olarak gönderilir
        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))] //NotFound filtresinin olabileceğini işaret eden attribute. Yani olmayan bir product'a ait bir id çekmeye çalışırsak NotFound hatası alabilmek için
        // GET /api/products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) //belirli bir id'ye göre product çekileceği zaman HttpGet isteği bu metodu çalıştırır.
        {
            var product = await _service.GetByIdAsync(id); //belirli bir id'ye göre asenkron olarak çekilen bir product, product değişkenine atılır.
            var productsDto = _mapper.Map<ProductDto>(product); //çekilmiş olan product mapleme işlemi ile birlikte ProductDto nesnesine dönüştürülür ve bu değer artık productsDto değişkenine atanır.
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, productsDto)); //çekilmiş ve dto nesnesine dönüştürülmüş olan product verisi CustomResponseDto ile özel bir yanıt olarak CreateActionResult metoduyla IActionResult tipinde http yanıtı ile client'a gösterilir
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto) //client yeni bir product eklemek istediği zaman controller HttpPost'a yönlendirir.
        {
            var product = await _service.AddAsync(_mapper.Map<Product>(productDto)); //ProductDto türündeki productDto verisi mapleme işlemi ile veri modeline dönüştürülerek asenkron bir şekilde veri tabanına eklenir.
            var productsDto = _mapper.Map<ProductDto>(product); // db'ye eklenmiş olan product mapleme işlemi ile tekrardan bir veri transfer nesnesi olan ProductDto nesnesine dönüştürülür
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(201, productsDto)); //201 durum kodu ile yani 'created' yeni product client'a 201 status code ile gösterilir
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto productDto) //Productlarla ilgili bir güncelleme işlemi yapılacaksa controller bu metoda yönlendirir
        {
             await _service.UpdateAsync(_mapper.Map<Product>(productDto)); //ProductUpdateDto türündeki productDto nesnesi veri modeline Product'a dönüştürülerek asenkron bir şekilde db'de güncellenir
          
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204)); //güncellenen Product nesnesi 204 http status code ile yani no content olarak işlemi tamamlar ve geriye bir şey döndürmez sadece arka planda güncelleme işlemini tamamlamış olur
        }
      
        // DELETE api/products/5
        [HttpDelete("{id}")] 
        public async Task<IActionResult> Remove(int id) //Belirli bir id'ye sahip product'ın silinme işlemi gerçekleşecekse ve client istek yaptıysa ProductsController isteği HttpDelete'e yönlendirir
        {
            var product = await _service.GetByIdAsync(id);  //id'si belli olan ürün getirilir ve product nesnesine bu ürün atılır

            await _service.RemoveAsync(product); //getirilen ürün _service ile birlikte db'den asenkron bir şekilde silinir
          
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204)); //silme işlemi gerçekleştirildikten sonra herhangi bir yanıt client'a dönülmez ve işlem tamamlanmış olur
        }

    }
}
