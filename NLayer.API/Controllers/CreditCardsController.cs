using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using NLayer.Service.Services;

namespace NLayer.API.Controllers
{
    public class CreditCardsController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly ICreditCardService _creditCardService;

        public CreditCardsController(IMapper mapper, ICreditCardService creditCardService)
        {
            _mapper = mapper;
            _creditCardService = creditCardService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCreditCardsWithUsers()
        {
            return CreateActionResult(await _creditCardService.GetCreditCardsWithUsers());
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var creditCards = await _creditCardService.GetAllAsync();
            var creditCardDtos = _mapper.Map<List<CreditCardDto>>(creditCards.ToList());
            return CreateActionResult(CustomResponseDto<List<CreditCardDto>>.Success(200, creditCardDtos));
        }

        [ServiceFilter(typeof(NotFoundFilter<CreditCard>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var creditCards = await _creditCardService.GetByIdAsync(id);
            var creditCardDtos = _mapper.Map<CreditCardDto>(creditCards);
            return CreateActionResult(CustomResponseDto<CreditCardDto>.Success(200, creditCardDtos));
        }

        [HttpPost]
        public async Task<IActionResult> Save(CreditCardDto creditCardDto)
        {
            var creditCards = await _creditCardService.AddAsync(_mapper.Map<CreditCard>(creditCardDto));
            var creditCardDtos = _mapper.Map<CreditCardDto>(creditCards);
            return CreateActionResult(CustomResponseDto<CreditCardDto>.Success(201, creditCardDtos));
        }

        [HttpPut]
        public async Task<IActionResult> Update(CreditCardUpdateDto creditCardDto)
        {
            await _creditCardService.UpdateAsync(_mapper.Map<CreditCard>(creditCardDto));

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var creditCard = await _creditCardService.GetByIdAsync(id);

            await _creditCardService.RemoveAsync(creditCard);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
    }
}
