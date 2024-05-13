namespace NLayer.Core.Models
{
    public class CreditCard : BaseEntity
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
