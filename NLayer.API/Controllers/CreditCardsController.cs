using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    public class CreditCardsController : CustomBaseController //Kredi Kartı işlemlerinin controller sınıfının tanımlanması. CustomBaseController sınıfından miras almasının sebebi her controller'da ayrı ayrı olarak route ve apicontroller attribute'larının tanımlanmasıyla uğraşmamaktır.
    {
        private readonly IMapper _mapper; //mapping işlemlerinin yapılabilmesi için IMapper türünde bir _mapper tanımlanması. readonly olarak tanımlanmasının sebebi sadece tanımlandıkları yerde yani sınıfın yapıcı metodunda(constructor) bu değişkenin kullanılabilmesini ve daha sonra değiştirilmesine izin vermemeyi sağlamaktır
        private readonly ICreditCardService _creditCardService; //kredi kartı business işlemleri yapılabilmesi için ICreditCardService türünde bir _creditCardService tanımlanması ve bu değer readonly'dir sadece tanımlandığı sınıfın yapıcı metodunda atanabilir ve daha sonra değiştirilemez

        public CreditCardsController(IMapper mapper, ICreditCardService creditCardService) //bu parametreler ile IMapper ve ICreditCardService işlemlerinin bağımlılıklarının dışarıdan enjekte edilmesi ve private olarak tanımlanmış olan değerlere bu değerlerin atanması sağlanır
        {
            _mapper = mapper; 
            _creditCardService = creditCardService;
        }

        [HttpGet("[action]")] //"[action]" olarak tanımlanmasının nedeni route isminin dinamik olarak metod adıyla aynı olmasını sağlamaktır. metod ismi değişirse route ismi de değişecektir
        public async Task<IActionResult> GetCreditCardsWithUsers()
        {
            return CreateActionResult(await _creditCardService.GetCreditCardsWithUsers()); //CreateActionResult metodu GetCreditCardWithUsers metodundan dönen sonucu alır ve bunu IActionResult türüne dönüştürerek HTTP yanıtı oluşturur.
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var creditCards = await _creditCardService.GetAllAsync(); //_creditCardService, kredi kartıyla ilgili business işlemleri için tanımlanmıştır ve asenkron olarak kredi kartı bilgilerini getirir ve creditCards değişkenine atar
            var creditCardDtos = _mapper.Map<List<CreditCardDto>>(creditCards.ToList()); //creditCards.ToList metoduyla creditCards listesi List<CreditCard> listesine dönüştürülür ve bu liste _mapper ile List<CreditCardDto> türüne dönüştürülür ve creditCardDtos değişkenine atılır.
            return CreateActionResult(CustomResponseDto<List<CreditCardDto>>.Success(200, creditCardDtos)); //CustomResponseDto özel bir yanıt DTO'su ve bu DTO, List<CreditCardDto> türünde bir veri içerir. 200 başarılı http durum kodudur ve bu durum kodunun içeriği creditCardDtos değişkenidir. CreateActionResult metodu ile birlikte bu CustomResponseDto bir http yanıtı olarak döndürülür.
        }

        [ServiceFilter(typeof(NotFoundFilter<CreditCard>))] //Eğer belirli bir id'ye sahip olan CreditCard nesnesi bulunamazsa NotFoundFilter<CreditCard> otomatik olarak bir hata yanıtı dönecektir
        [HttpGet("{id}")] //Belirli bir id'ye sahip olan nesne için http get isteğidir
        public async Task<IActionResult> GetById(int id) //asenkron olarak çalışan bu GetById metodu belirli bir id'ye göre Task<IActionResult> türünde bir yanıt döndürür
        {
            var creditCards = await _creditCardService.GetByIdAsync(id); //_creditCardService, kredi kartıyla ilgili işlemlerdir ve belirli bir id'ye göre asenkron olarak getirilen kredi kartı bilgisini creditCards değişkenine atar
            var creditCardDtos = _mapper.Map<CreditCardDto>(creditCards); //creditCards nesnesindeki değişken _mapper aracılığıyla CreditCardDto veri transfer nesnesine dönüştürülür ve bu nesne creditCardDtos nesnesine atanır
            return CreateActionResult(CustomResponseDto<CreditCardDto>.Success(200, creditCardDtos)); //CreditCardDto için özel bir yanıt olan CustomResponseDto, CreateActionResult ile birlikte bir IActionResult ile http yanıtı olarak client'a yanıt olarak gönderilir
        }

        [HttpPost]
        public async Task<IActionResult> Save(CreditCardDto creditCardDto) //CreditCardDto nesnesi http post isteğiyle alınır bu nesne kredi kartı bilgilerini içerir
        {
            var creditCards = await _creditCardService.AddAsync(_mapper.Map<CreditCard>(creditCardDto)); //kredi kartı bilgilerini içeren creditCardDto nesnesi CreditCard türüne dönüştürülür(mapping) ardından bu değer veri tabanına eklenir
            var creditCardDtos = _mapper.Map<CreditCardDto>(creditCards); //creditcards değişkenindeki kredi kartı bilgileri CreditCardDto nesnesine dönüştürülür ve creditCardDtos değişkenine atanır
            return CreateActionResult(CustomResponseDto<CreditCardDto>.Success(201, creditCardDtos)); //CreateActionResult ile CustomResponseDto özel yanıtı döndürülür
        }

        [HttpPut]
        public async Task<IActionResult> Update(CreditCardUpdateDto creditCardDto) //Update edilecek credit card bilgileri CreditCardUpdateDto ile creditCardDto üzerinden alınır
        {
            await _creditCardService.UpdateAsync(_mapper.Map<CreditCard>(creditCardDto)); //creditCardDto veri transfer nesnesi ile alınan kredi kartı bilgileri _mapper ile CreditCard veri modeline dönüştürülür ve dönüştürülen CreditCard asenkron bir şekilde güncellenir

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204)); //Güncellenen kredi kartı bilgileri CustomResponseDto özel yanıt veri transferi ile NoContentDto türünde geriye bir şey dönmeyecek şekilde CreateActionResult yardımcı metodu ile IActionResult tipinde client'a döndürülür
        }

        [HttpDelete("{id}")] //client tarafından silme işlemi isteği bu HttpDelete attribute'a düşer, bu attribute gelen isteği action metoda yönlendirir ve bu istekleri yönlendiren de route'dur. controller sadece route tarafından belirlenen işlemleri yapar. belirli bir veri id'sine göre olduğu dikkatimizden kaçmamalı
        public async Task<IActionResult> Remove(int id) //silinecek olan kredi kartının id si HttpDelete isteği ile alınır
        {
            var creditCard = await _creditCardService.GetByIdAsync(id); //id'si alınan kredi kartının _creditCardService.GetByIdAsync(id) ile asenkron olarak getirilmesi ve creditCard değişkenine atanması sağlanır

            await _creditCardService.RemoveAsync(creditCard); //kredi kartı işlemlerinin yapıldığı _creditCardService ile bir önceki satırda id'si alınan kredi kartının veri tabanından silme işlemi tamamlanır ve asenkron olarak bu işlem yapılır

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204)); //NoContent olacak şekilde 204 durum kodlu http yanıtı client'a döndürülür.
        }
    }
}
