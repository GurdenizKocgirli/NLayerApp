using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    public class AddressesController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IAddressService _addressService;

        public AddressesController(IMapper mapper, IAddressService addressService)
        {
            _mapper = mapper;
            _addressService = addressService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAddressesWithUsers()
        {
            return CreateActionResult(await _addressService.GetAddressesWithUsers());
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var addresses = await _addressService.GetAllAsync();
            var addressesDtos = _mapper.Map<List<AddressDto>>(addresses.ToList());
            return CreateActionResult(CustomResponseDto<List<AddressDto>>.Success(200, addressesDtos));
        }

        [ServiceFilter(typeof(NotFoundFilter<Address>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var addresses = await _addressService.GetByIdAsync(id);
            var addressesDtos = _mapper.Map<AddressDto>(addresses);
            return CreateActionResult(CustomResponseDto<AddressDto>.Success(200, addressesDtos));
        }

        [HttpPost]
        public async Task<IActionResult> Save(AddressDto addressDto)
        {
            var addresses = await _addressService.AddAsync(_mapper.Map<Address>(addressDto));
            var addressesDtos = _mapper.Map<AddressDto>(addresses);
            return CreateActionResult(CustomResponseDto<AddressDto>.Success(201, addressesDtos));
        }

        [HttpPut]
        public async Task<IActionResult> Update(AddressUpdateDto addressDto)
        {
            await _addressService.UpdateAsync(_mapper.Map<Address>(addressDto));

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var address = await _addressService.GetByIdAsync(id);

            await _addressService.RemoveAsync(address);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

    }
}
