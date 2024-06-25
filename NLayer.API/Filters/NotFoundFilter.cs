using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Services;

namespace NLayer.API.Filters
{
    public class NotFoundFilter<T> :IAsyncActionFilter where T : BaseEntity //belirli bir entity türünün "T" var olup olmadığını kontrol eden ve yoksa 404 not found döndüren bir aksiyon filtresi
    {

        private readonly IService<T> _service; //_service generic bir servis nesnesidir ve IService<T> arayüzünü uygular

        public NotFoundFilter(IService<T> service) //_service nesnesinin bağımlılığının constructor'da verilmesi. Dışarıdan verilmesini sağlamak ileride farklı bir service işlemi yapmak istediğimiz zaman buradan direkt olarak değiştirmemizi ve projenin tümünü değiştirmeden değişiklik sağlamamıza yarar
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) //bu metod aksiyon çalıştırılmadan önce ve sonra çalışacak kodları belirler
        {

            var idValue = context.ActionArguments.Values.FirstOrDefault(); //aksiyon metoduna geçirilen ilk argümanı alır ve idValue değerine atar

            if(idValue == null) //idValue null ise aksiyon metodunun çalıştırılmasına izin verilir
            {
                await next.Invoke();
                return;
            }

            var id = (int)idValue; //idValue null değilse bu değeri bir 'int' e dönüştürür
            var anyEntity = await _service.AnyAsync(x => x.Id == id); //_service.AnyAsync ile belirtilen id'ye sahip bir entity olup olmadığı kontrol edilir

            if(anyEntity) //eğer entity varsa aksiyon metodunun çalıştırılmasına izin verilir
            {
                await next.Invoke();
                return;
            }

            context.Result = new NotFoundObjectResult(CustomResponseDto<NoContentDto>.Fail(404, $"{typeof(T).Name}({id}) not found")); //eğer entity yoksa context.Result bir "NotFoundObjectResult" ile ayarlanır, bu 404 not found http yanıtı döndürecektir
                                                                                                                                       //CustomResponseDto<NoContentDto>.Fail(404, $"{typeof(T).Name}({id}) not found") ifadesi, özelleştirilmiş bir hata mesajı oluşturur ve entity'nin türü ve ID'sini içerir
        }

    }
}
