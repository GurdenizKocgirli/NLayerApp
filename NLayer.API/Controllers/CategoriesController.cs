using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    public class CategoriesController : CustomBaseController //kategoriler ile ilgili controller işlemlerinin yapıldığı class
    {
<<<<<<< HEAD
        private readonly ICategoryService _categoryService; //kategori işlemlerinin yapıldığı business işleri
        private readonly IMapper _mapper; //mapping işlemlerinin yapıldığı business işleri
        public CategoriesController(ICategoryService categoryService, IMapper mapper) //ctor içerisinde bağımlılıkların dışarıdan verilmesi
=======
        private readonly ICategoryService _categoryService; //kategori servis işlemlerinin yapıldığı ve atandığı değişken
        private readonly IMapper _mapper; // mapping işlemleri için tanımlanmış olan değişken
        public CategoriesController(ICategoryService categoryService, IMapper mapper) //private olarak tanımlanmış olan _categoryService ve _mapper değişkenlerine bağımlılıkların yapıcı metod içerisinde dışarıdan verilmesi
>>>>>>> 64e5beb0f53e6f1392849d3b784b2a70a7b25c57
        {
            _categoryService = categoryService; 
            _mapper = mapper;
        }

<<<<<<< HEAD
        [HttpGet] //Client get isteği yaptığı zaman route controller'a bu attribute'a yönlendirmesi gerektiğini söyler ve bu attribute da isteği aşağıdaki action metoduna yönlendirir
        public async Task<IActionResult> GetAll() 
        {

            var categories = await _categoryService.GetAllAsync(); //asenkron bir şekilde _categoryService ile birlikte kategoriler getirilir ve categories değişkenine atılır

            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList()); //getirilmiş olan kategoriler bir listeye döndürülür ve mapping işlemi ile birlikte List<Category> ' den List<CategoryDto> veri transfer nesnesine dönüştürülerek categoriesDto değişkenine atılır

            return CreateActionResult(CustomResponseDto<List<CategoryDto>>.Success(200,categoriesDto)); //IActionResult tipinde bir yardımcı metod olan CreateActionResult ile birlikte CustomResponseDto yanıtı client'a döndürülür
=======
        [HttpGet] //Bütün kategorilerin getirilmesi istendiği zaman aşağıdaki metod çalışır
        public async Task<IActionResult> GetAll()
        {

            var categories = await _categoryService.GetAllAsync(); //bütün kategoriler asenkron olarak db'den çekildikten sonra categories değişkenine atanır

            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList()); //çekilmiş olan List<Category> listesi _mapper ile List<CategoryDto>'ya dönüştürülür ve elde edilen sonuç categoriesDto değişkenine atanır

            return CreateActionResult(CustomResponseDto<List<CategoryDto>>.Success(200,categoriesDto)); //CategoryDto türündeki liste özel bir yanıt dto'su olan CustomResponseDto ile CreateActionResult yardımıyla IActionResult türünde döndürülür
>>>>>>> 64e5beb0f53e6f1392849d3b784b2a70a7b25c57
        
        }

        // api/categories/GetSingleCategoryByIdWithProducts/2
<<<<<<< HEAD
        [HttpGet("[action]/{categoryId}")] //belirli bir categoryId'ye göre get isteği yapıldığı zaman bu attribute aşağıdaki action metodun çalışmasını sağlar
        public async Task<IActionResult> GetSingleCategoryByIdWithProducts(int categoryId) 
        {
            return CreateActionResult(await _categoryService.GetSingleCategoryByIdWithProductsAsync(categoryId)); //categoryId'si alınan kategori sahip olduğu productlar ile birlikte istemciye CreateActionResult yardımcı metoduyla IActionResult olarak döndürülür
=======
        [HttpGet("[action]/{categoryId}")] //Belirli bir kategorinin sahip olduğu ürünleri görmek için kullanılan http isteği
        public async Task<IActionResult> GetSingleCategoryByIdWithProducts(int categoryId) 
        {
            return CreateActionResult(await _categoryService.GetSingleCategoryByIdWithProductsAsync(categoryId)); //client tarafından gönderilen kategori id'si _categoryService ile birlikte asenkron olarak db'den çekilir ve CreateActionResult yardımcı metoduyla IActionResult türünde http yanıtı olarak client'a gösterilir
>>>>>>> 64e5beb0f53e6f1392849d3b784b2a70a7b25c57
        }
    }
}
