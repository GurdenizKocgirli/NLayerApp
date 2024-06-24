using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    public class AddressesController : CustomBaseController //Adresler ile ilgili http yanıtları burada düzenlenir
    {
        private readonly IMapper _mapper; //mapping işlemleri için IMapper türünde bir _mapper oluşturulması ve readonly olarak tanımlanmasının nedeni sadece tanımlanmış olduğu sınıfın yapıcı metodunda bir kez atanabilecek olması ve bir daha değiştirilemeyecek olmasıdır
        private readonly IAddressService _addressService; //business işlemi olan ve adres işlemlerini düzenleyen IAddressService türünde bir _addressService oluşturulması, readonly olarak tanımlanmıştır çünkü sadece yapıcı metodda tanımlamak ve bir daha değiştirilmemesini istiyoruz

        public AddressesController(IMapper mapper, IAddressService addressService)
        {
            _mapper = mapper; //IMapper mapper parametresi bağımlılığını dışarıdan _mapper'a atayarak DI işlemini gerçekleştirmiş oluyoruz bu kodumuzun esnek olmasını sağlar
            _addressService = addressService; //IAddressService addressService parametresinin bağımlılığı dışarıdan verilir ve private olarak tanımlanmış _adressService değerine atanır
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAddressesWithUsers() //Adres bilgileriyle birlikte kullanıcıların istendiği httpget isteğine burada bu metod ile cevap verilir
        {
            return CreateActionResult(await _addressService.GetAddressesWithUsers()); //CreateActionResult NonAction olarak tanımlanmış bir IActionResult türüdür yani bir eylem metodu değil de bir yardımcı metoddur. Bu yardımcı metod IActionResult türünde yanıt döndürülmesini sağlar
        }

        [HttpGet]
        public async Task<IActionResult> All() //sadece bütün adreslerin getirilmesini sağlayan metod
        {
            var addresses = await _addressService.GetAllAsync(); //asenkron olarak getirilen adres bilgileri addresses değişkenine atanır
            var addressesDtos = _mapper.Map<List<AddressDto>>(addresses.ToList()); //atanmış olan adres bilgileri List<Address> şeklindedir ve bu veri transfer modeli olan List<AdressDto>'ya dönüştürülür ve addressesDtos nesnesine atanır
            return CreateActionResult(CustomResponseDto<List<AddressDto>>.Success(200, addressesDtos)); //DTO nesnesine dönüştürülmüş olan adres bilgileri CreateActionResult metodu yardımıyla CustomResponseDto türünde yanıt olarak client'a http yanıtı olarak döndürülür
        }

        [ServiceFilter(typeof(NotFoundFilter<Address>))] //İstenilen adres id'sinin bulunamayacağı ihtimaline karşılık olarak NotFoundFilter attribute'u Address için belirtilmiştir
        [HttpGet("{id}")] //belirli bir id'ye göre client httpget isteği yaparsa aşağıdaki metodun çalıştırılacağını bize söyler
        public async Task<IActionResult> GetById(int id) //int id parametresi belirli bir id'nin kullanılacağını belirtir ve bu metodun asenkron olarak çalışacağı ayrıca bir Task<IActionResult> sonucu döndüreceğini belirtir. Task, bir asenkron işlemin tamamlanmasını beklemek için kullanılır. 
        {
            var addresses = await _addressService.GetByIdAsync(id); //await, bir id'ye göre getirilen adresin getirilirken diğer işlemleri bloklamayacağını belirtir.
            var addressesDtos = _mapper.Map<AddressDto>(addresses); //_mapper ile id'ye göre getirilmiş olan adres bilgisi Address'ten AdressDto nesnesine dönüştürülür ve addressesDtos değişkenine atanır
            return CreateActionResult(CustomResponseDto<AddressDto>.Success(200, addressesDtos)); //CustomResponseDto<AddressDto> türünde başarılı bir http yanıtı CreateActionResult yardımcı metoduyla IActionResult türünde client'a http yanıtı olarak gösterilir
        }

        [HttpPost] //yeni bir adres ekleneceği zaman verilecek olan http yanıtı
        public async Task<IActionResult> Save(AddressDto addressDto) //AdressDto türünde bir addressDto parametresi metodumuzda kullanılır
        {
            var addresses = await _addressService.AddAsync(_mapper.Map<Address>(addressDto)); //alınmış olan addresDto nesnesi veri modeli olan Address'e dönüştürülür ve veri tabanına asenkron olarak eklenir ve addresses değişkenine atanır
            var addressesDtos = _mapper.Map<AddressDto>(addresses); //Address veri modeline dönüştürülmüş olan ve veri tabanına eklenmiş olan addresses değişkeni tekrardan veri transfer nesnesi olan AdressDto'ya dönüştürülür ve addressesDto değişkenine atanır
            return CreateActionResult(CustomResponseDto<AddressDto>.Success(201, addressesDtos)); //Özel bir veri transferi yanıtı olarak oluşuturulmuş olan CustomResponseDto, bir önceki satırda dto nesnesine dönüştürülmüş olan AddressDto başarılı yanıtını CreateActionResult alır ve IActionResult türünde bir http yanıtı ile client'a gösterir
        }

        [HttpPut] //Adresler ile ilgili güncelleme işlemleri isteği Route tarafından buraya yönlendirilir ve aşağıdaki metodun çalışmasına olanak sağlar
        public async Task<IActionResult> Update(AddressUpdateDto addressDto) //asenkron olarak Task türünde bir IActionResult yanıtı dönen metod
        {
            await _addressService.UpdateAsync(_mapper.Map<Address>(addressDto)); //güncellenecek olan addressDto nesnesi Address veri modeline dönüştürülür ve await ile diğer işlemleri bloklamadan Update edilir

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204)); //CustomResponseDto<NoContentDto> 204 durumlu başarı yanıtı CreateActionResult ile alınır ve IActionResult türünde döndürülür. 204 durum kodu olduğu için herhangi bir şey client'a görsel olarak gösterilmez sadece işlem tamamlanmış olur
        }

        [HttpDelete("{id}")] //belirli bir id'ye göre adres silme işlemleri route tarafından buraya yönlendirilir ve attribute aşağıdaki metodun çalışacağını söyler
        public async Task<IActionResult> Remove(int id) //belirli bir id'ye göre client tarafından silinmesi istenilen id  parametre olarak metoda verilir
        {
            var address = await _addressService.GetByIdAsync(id); //alınan id _addressService ile birlikte ilk önce veri tabanından asenkron olarak çekilir daha sonra address değişkenine atanır

            await _addressService.RemoveAsync(address); //_adressService business işlemi yani adreslerle ilgili olarak tanımlanmış olan adres işlemleri değişkeni ile birlikte bir önceki satırda alınmış olan adress değişkeni asenkron bir şekilde db'den silinir

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204)); //NoContentDto olarak özel bir yanıt olan CustomResponseDto başarılı nesnesi CreateActionResult yardımcı metoduyla IActionResult tipinde client'a döndürülür. Herhangi bir şey gösterilmez
        }

    }
}
