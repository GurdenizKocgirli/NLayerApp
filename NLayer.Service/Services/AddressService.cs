using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services
{
    public class AddressService : Service<Address>, IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        public AddressService(IGenericRepository<Address> repository, IUnitOfWork unitOfWork, IMapper mapper, IAddressRepository addressRepository) : base(repository, unitOfWork)
        {
            _mapper = mapper;
            _addressRepository = addressRepository;
        }

        public async Task<CustomResponseDto<List<AddressesWithUsersDto>>> GetAddressesWithUsers()
        {
            var addresses = await _addressRepository.GetAddressesWithUsers();
            var addressesDto = _mapper.Map<List<AddressesWithUsersDto>>(addresses);

            return CustomResponseDto<List<AddressesWithUsersDto>>.Success(200, addressesDto);
        }
    }
}
