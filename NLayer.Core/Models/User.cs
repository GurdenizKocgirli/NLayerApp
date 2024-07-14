using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Models
{
    public class User : BaseEntity //Kullanıcı bilgilerinin yer aldığı User sınıfımız
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public ICollection<Address> Addresses { get; set; } //Bir User'ın birden fazla Adres bilgisine sahip olabileceğini belirten bir ICollection değişkeni
        public ICollection<CreditCard> CreditCards { get; set; } //Bir User'ın birden fazla Kredi Kartı bilgisine sahip olabileceğini belirtir
    }
}
