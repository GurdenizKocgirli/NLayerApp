using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")] // gelen http isteklerinin hangi controller ve action methoduna yönlendireceğini belirler
    [ApiController] //model doğrulama kurallarına uymayan veriler varsa ModelState.IsValid oto olarak false döner. Manuel olarak girmeye gerek yoktur. 
    //Model geçerli değilse API otomatik olarak 400 (bad request) yanıtı döner.
    public class CustomBaseController : ControllerBase
    {

        [NonAction] //metodun bir action olmadığını sadece yardımcı bir metot olduğunu belirtir.
        public IActionResult CreateActionResult<T>(CustomResponseDto<T> response)
            //CreateActionResult metodunun amacı CustomResponseDto<T> tipindeki yanıtları IActionResult türüne dönüştürmektir.API'ler için standart bir
            //yanıt formatı oluşturmamızı sağlar.
        {
            if (response.StatusCode == 204) // yanıtın durum kodunun 204(no content) olup olmadığını kontrol eder
                //eğer yanıtın durum kodu 204 ise ObjectResult nesnesi null değerine dönüştürülür ve durum kodu 204 olarak ayarlanır.
                return new ObjectResult(null)
                {
                    StatusCode = response.StatusCode
                };

            return new ObjectResult(response) //eğer durum kodu 204 değil ise ObjectResult nesnesi response değeriyle oluşturulur ve durum kodu response.StatusCode olarak ayarlanır.
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
