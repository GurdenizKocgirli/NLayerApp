using System.Text.Json.Serialization;

namespace NLayer.Core.DTOs
{
    public class CustomResponseDto<T>
    {
        public T Data { get; set; } //API yanıtında döndürülen gerçek veriyi tutar, T type olduğundan veri tipi dinamik olarak belirlenebilir
        [JsonIgnore] //istemcinin görmesini istemediğimiz için kullanırız
        public int StatusCode { get; set; } //http durum kodunu tutar
        public List<String> Errors { get; set; } //oluşabilecek hata mesajlarını tutar

        public static CustomResponseDto<T> Success(int statusCode, T data)
        {
            return new CustomResponseDto<T> { Data = data, StatusCode = statusCode };
        }
        public static CustomResponseDto<T> Success(int statusCode)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode };
        }

        public static CustomResponseDto<T> Fail(int statusCode, List<string> errors)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Errors = errors };
        }

        public static CustomResponseDto<T> Fail(int statusCode, string error)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Errors = new List<string> { error } };
        }
    }
}
