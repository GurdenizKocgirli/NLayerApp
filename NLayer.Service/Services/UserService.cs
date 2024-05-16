using AutoMapper;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;

namespace NLayer.Service.Services
{
    public class UserService : Service<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IGenericRepository<User> repository, IUnitOfWork unitOfWork, IMapper mapper, IUserRepository userRepository) : base(repository, unitOfWork)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<CustomResponseDto<UsersWithCreditCardsAndAddressesDto>> GetSingleUserByIdWithCreditCardsAndUsersAsync(int userId)
        {
            var user = await _userRepository.GetSingleUserWithCreditCardsAndAddressesAsync(userId);

            var userDto = _mapper.Map<UsersWithCreditCardsAndAddressesDto>(user);

            return CustomResponseDto<UsersWithCreditCardsAndAddressesDto>.Success(200, userDto);
        }
    }
}
