using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    public class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService _categoryService; //kategori işlemlerinin yapıldığı business işleri
        private readonly IMapper _mapper; //mapping işlemlerinin yapıldığı business işleri
        public CategoriesController(ICategoryService categoryService, IMapper mapper) //ctor içerisinde bağımlılıkların dışarıdan verilmesi
        {
            _categoryService = categoryService; 
            _mapper = mapper;
        }

        [HttpGet] //Client get isteği yaptığı zaman route controller'a bu attribute'a yönlendirmesi gerektiğini söyler ve bu attribute da isteği aşağıdaki action metoduna yönlendirir
        public async Task<IActionResult> GetAll() 
        {

            var categories = await _categoryService.GetAllAsync(); //asenkron bir şekilde _categoryService ile birlikte kategoriler getirilir ve categories değişkenine atılır

            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList()); //getirilmiş olan kategoriler bir listeye döndürülür ve mapping işlemi ile birlikte List<Category> ' den List<CategoryDto> veri transfer nesnesine dönüştürülerek categoriesDto değişkenine atılır

            return CreateActionResult(CustomResponseDto<List<CategoryDto>>.Success(200,categoriesDto)); //IActionResult tipinde bir yardımcı metod olan CreateActionResult ile birlikte CustomResponseDto yanıtı client'a döndürülür
        
        }

        // api/categories/GetSingleCategoryByIdWithProducts/2
        [HttpGet("[action]/{categoryId}")] //belirli bir categoryId'ye göre get isteği yapıldığı zaman bu attribute aşağıdaki action metodun çalışmasını sağlar
        public async Task<IActionResult> GetSingleCategoryByIdWithProducts(int categoryId) 
        {
            return CreateActionResult(await _categoryService.GetSingleCategoryByIdWithProductsAsync(categoryId)); //categoryId'si alınan kategori sahip olduğu productlar ile birlikte istemciye CreateActionResult yardımcı metoduyla IActionResult olarak döndürülür
        }
    }
}
