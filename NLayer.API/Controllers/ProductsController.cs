using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    public class ProductsController : CustomBaseController //CustomBaseController sınıfı base bir class olduğu için içerisinde zaten Route mekanizması vardır bu yüzden ProductsController class'ında tekrardan bir route tanımlamamıza gerek yoktur. Route gelen isteklerin hangi Controller sınıfına yönlendireceğine karar veren bir özelliktir
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

    [ServiceFilter(typeof(NotFoundFilter<Product>))]
        // GET /api/products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            var productsDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, productsDto));
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            var product = await _service.AddAsync(_mapper.Map<Product>(productDto));
            var productsDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(201, productsDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto productDto)
        {
             await _service.UpdateAsync(_mapper.Map<Product>(productDto));
          
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
      
        // DELETE api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _service.GetByIdAsync(id);

            await _service.RemoveAsync(product);
          
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

    }
}
