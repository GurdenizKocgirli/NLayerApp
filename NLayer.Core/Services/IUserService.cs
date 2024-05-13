using NLayer.Core.DTOs;
using NLayer.Core.Models;

namespace NLayer.Core.Services
{
    public interface IUserService : IService<User>
    {
        public Task<CustomResponseDto<UsersWithCreditCardsAndAddresses>> GetSingleUserByIdWithCreditCardsAndUsersAsync(int userId);
    }
}
