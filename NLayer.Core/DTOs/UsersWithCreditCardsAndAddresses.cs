using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.DTOs
{
    public class UsersWithCreditCardsAndAddresses : UserDto
    {
        public List<AddressDto> Address { get; set; }
        public List<CreditCardDto> CreditCard { get; set; }
    }
}
