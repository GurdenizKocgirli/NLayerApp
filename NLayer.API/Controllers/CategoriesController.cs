using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    public class CategoriesController : CustomBaseController //kategoriler ile ilgili controller işlemlerinin yapıldığı class
    {
        private readonly ICategoryService _categoryService; //kategori servis işlemlerinin yapıldığı ve atandığı değişken
        private readonly IMapper _mapper; // mapping işlemleri için tanımlanmış olan değişken
        public CategoriesController(ICategoryService categoryService, IMapper mapper) //private olarak tanımlanmış olan _categoryService ve _mapper değişkenlerine bağımlılıkların yapıcı metod içerisinde dışarıdan verilmesi
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet] //Bütün kategorilerin getirilmesi istendiği zaman aşağıdaki metod çalışır
        public async Task<IActionResult> GetAll()
        {

            var categories = await _categoryService.GetAllAsync(); //bütün kategoriler asenkron olarak db'den çekildikten sonra categories değişkenine atanır

            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList()); //çekilmiş olan List<Category> listesi _mapper ile List<CategoryDto>'ya dönüştürülür ve elde edilen sonuç categoriesDto değişkenine atanır

            return CreateActionResult(CustomResponseDto<List<CategoryDto>>.Success(200,categoriesDto)); //CategoryDto türündeki liste özel bir yanıt dto'su olan CustomResponseDto ile CreateActionResult yardımıyla IActionResult türünde döndürülür
        
        }

        // api/categories/GetSingleCategoryByIdWithProducts/2
        [HttpGet("[action]/{categoryId}")] //Belirli bir kategorinin sahip olduğu ürünleri görmek için kullanılan http isteği
        public async Task<IActionResult> GetSingleCategoryByIdWithProducts(int categoryId) 
        {
            return CreateActionResult(await _categoryService.GetSingleCategoryByIdWithProductsAsync(categoryId)); //client tarafından gönderilen kategori id'si _categoryService ile birlikte asenkron olarak db'den çekilir ve CreateActionResult yardımcı metoduyla IActionResult türünde http yanıtı olarak client'a gösterilir
        }
    }
}
