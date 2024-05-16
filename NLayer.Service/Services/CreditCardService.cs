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
    public class CreditCardService : Service<CreditCard>, ICreditCardService
    {
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly IMapper _mapper;
        public CreditCardService(IGenericRepository<CreditCard> repository, IUnitOfWork unitOfWork, ICreditCardRepository creditCardRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _mapper = mapper;
            _creditCardRepository = creditCardRepository;
        }

        public async Task<CustomResponseDto<List<CreditCardsWithUsersDto>>> GetCreditCardsWithUsers()
        {
            var creditCards = await _creditCardRepository.GetCreditCardsWithUsers();
            var creditCardsDto = _mapper.Map<List<CreditCardsWithUsersDto>>(creditCards);

            return CustomResponseDto<List<CreditCardsWithUsersDto>>.Success(200, creditCardsDto);
        }
    }
}
