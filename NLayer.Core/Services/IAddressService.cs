using NLayer.Core.DTOs;
using NLayer.Core.Models;

namespace NLayer.Core.Services
{
    public interface IAddressService : IService<Address>
    {
        Task<CustomResponseDto<List<AddressesWithUsersDto>>> GetAddressesWithUsers();
    }
}
