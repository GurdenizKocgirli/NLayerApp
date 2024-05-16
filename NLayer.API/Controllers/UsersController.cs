using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Services;
using NLayer.Service.Services;
using System.Collections.Generic;

namespace NLayer.API.Controllers
{
    public class UsersController : CustomBaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            var usersDto = _mapper.Map<List<UserDto>>(users.ToList());

            return CreateActionResult(CustomResponseDto<List<UserDto>>.Success(200, usersDto));
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetSingleUserByIdWithCreditCardsAndUsersAsync(int userId)
        {

            return CreateActionResult(await _userService.GetSingleUserByIdWithCreditCardsAndUsersAsync(userId));

        }
    }
}
