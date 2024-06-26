using Microsoft.AspNetCore.Diagnostics;
using NLayer.Core.DTOs;
using NLayer.Service.Exceptions;
using System.Text.Json;

namespace NLayer.API.Middlewares
{
    public static class UseCustomExceptionHandler //UseCustomExceptionHandler bir Middleware'dir. Middleware'ler, isteklerin gelmesi ve yanıt olarak gönderilmesi sürecinde ara katman görevi görürler. Her bir middleware kendi görevini üstlenir ve bir sonraki middleware'e doğru olacak şekilde zincirleme bir şekilde çalışırlar ve en sonunda da http yanıtı client'a gönderilir.
    {

        public static void UseCustomException(this IApplicationBuilder app) //IApplicationBuilder interface'i middleware'lerin yapılandırılmasını sağlayan, sıralamasını düzenleyen yapıdır
        {//this IApplicationBuilder app parametresi IApplicationBuilder interface'ine yeni işlevsellikler eklemek için kullanılır ve this ile bu türün örneği belirtilir
            //bu yapılan işlem genişletilmiş yöntem olarak tanımlanır. bir class veya interface'i genişletmek veya yeni işlevsellik eklemek için kullanılır
            app.UseExceptionHandler(config => //app.UseExceptionHandler ile Asp.Net Core uygulamamıza bir hata yönetimi middleware'i eklemek için kullanırız ve app parametresi üzerinden ilerlenir, config parametresi UseExceptionHandler yöntemi tarafından sağlanan yapılandırma ayarlarını içerir    
            {

                config.Run(async context => //config.Run ile hata durumu olursa çalışacak işlevselliktir. context hata detaylarını içerir
                {
                    context.Response.ContentType = "application/json"; //istemciye gönderilecek yanıt türünün JSON formatında olacağını belirtir

                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>(); //IExceptionHandlerFeature ile oluşan exception hakkında context.Features'tan detaylar alınır

                    var statusCode = exceptionFeature.Error switch //exceptionFeature içerisine atılan hata detayı, hata durumunu belirlemek için bir switch bloğuna alınır ve hata durumu belirlenip statusCode değişkenine atanır
                    {
                        ClientSideException => 400,
                        NotFoundExcepiton=> 404,
                        _ => 500
                    };
                    context.Response.StatusCode = statusCode; //client'a gönderilecek olan http durum kodu statusCode'dan context.Response.StatusCode değişkenine atanır


                    var response = CustomResponseDto<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message); //belirlenmiş olan statusCode ve hata mesajı ile birlikte CustomResponseDto özel yanıtı oluşturulur


                    await context.Response.WriteAsync(JsonSerializer.Serialize(response)); //oluşturulmuş olan response değişkeninde yer alan hata mesajı json yanıt formatına dönüştürülerek client'a gönderilir

                });

            });

        }
    }
}
