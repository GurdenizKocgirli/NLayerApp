using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;

namespace NLayer.API.Filters
{
 
    public class ValidateFilterAttribute :ActionFilterAttribute //ValidateFilterAttribute, bir web uygulamasından gelen isteklerin doğruluğunu kontrol etmek ve hatalı istekleri ele almak için kullanılan bir sınıftır
    { //Bu sınıf bir Action Filter olarak adlandırılır ve belirli bir işlemden önce veya sonra çalışabilir
        //ActionFilterAttribute Asp.Net Core'da yerleşik bir sınıftır ve bir eylem çalıştırılmadan önce veya sonra özel mantık eklemek için kullanılır

        public override void OnActionExecuting(ActionExecutingContext context) //OnActionExecuting, http isteği controller tarafından işlenmeden önce çalıştırılır. Bu metodun bunu yapmasının amacı ise http isteğinin doğruluğunu kontrol etmektir. Yani controller tarafından client'a bir http yanıtı verilmeden önce veya işlenmeden önce isteğin doğruluğunu kontrol etmeye yarar
        {//ActionExecutingContext context parametresi yapılmış olan istek hakkında bilgi sağlar ve içinde ModelState gibi doğrulama sonuçlarını tutan nesneler bulunur
           if(!context.ModelState.IsValid) //ModelState, bir http isteğinin verilerinin doğrulanıp doğrulanmadığını tutar. Bu condition da ModelState'in geçerli olup olmadığını kontrol eder
            {
                var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList(); //ModelState nesnesindeki hataları toplamak ve hata mesajlarını bir listeye dönüştürmek amacıyla kullanılır. Bu liste errors değişkenine atanmıştır
                //context.ModelState.Values, ModelState nesnesindeki tüm değerleri yani model doğrulama sonuçlarını alır
                //SelectMany, Her bir 'ModelState' değeri için tüm hata koleksiyonlarını düzleştirir
                //Select, her bir hatadan hata mesajını alır
                //ToList(), sonuçları bir listeye dönüştürür
                context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Fail(400, errors)); //Model doğrulama hataları nedeniyle oluşan bir bad request http yanıtı döndürmek için kullanılır
                //context.Result, şu anki işlemin sonucunu temsil eder. Bu genellikle bir http yanıtı türünde bir nesnedir
                //new BadRequestObjectResult, yeni bir BadRequestObjectResult nesnesi oluşturur. Bu nesne http 400 bad request yanıtını temsil eder.
            }
        }
    }
}
