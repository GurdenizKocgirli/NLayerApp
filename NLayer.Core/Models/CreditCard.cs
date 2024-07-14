namespace NLayer.Core.Models
{
    public class CreditCard : BaseEntity
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } //CreditCard nesnesinin sadece bir adet User'a ait olacağını belirtir
    }
}
