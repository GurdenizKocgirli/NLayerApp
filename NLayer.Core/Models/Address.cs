namespace NLayer.Core.Models
{
    public class Address : BaseEntity //Adres bilgileri için entity sınıfımızın oluşturulması
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } //Her adresin sadece bir adet User ile bağlantılı olabileceğini belirtir
    }
}
