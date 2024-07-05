using System.Text.Json.Serialization;

namespace NLayer.Core.DTOs
{
    public class CustomResponseDto<T>
    {
        public T Data { get; set; } //API yanıtında döndürülen gerçek veriyi tutar, T type olduğundan veri tipi dinamik olarak belirlenebilir
        [JsonIgnore] //istemcinin görmesini istemediğimiz için kullanırız
        public int StatusCode { get; set; } //http durum kodunu tutar
        public List<String> Errors { get; set; } //oluşabilecek hata mesajlarını tutar

        public static CustomResponseDto<T> Success(int statusCode, T data) //client'a durum kodu ile birlikte veriyi de dönmek istediğimiz zaman kullanılır
        {
            return new CustomResponseDto<T> { Data = data, StatusCode = statusCode };
        }
        public static CustomResponseDto<T> Success(int statusCode) //yanıt olarak sadece durum kodu döndürülür
        {
            return new CustomResponseDto<T> { StatusCode = statusCode };
        }

        public static CustomResponseDto<T> Fail(int statusCode, List<string> errors) //durum kodu ile birlikte hata mesajları döndürülür
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Errors = errors };
        }

        public static CustomResponseDto<T> Fail(int statusCode, string error) //durum kodu ile birlikte tek bir hata mesajı döndürülür
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Errors = new List<string> { error } };
        }
    }
}
