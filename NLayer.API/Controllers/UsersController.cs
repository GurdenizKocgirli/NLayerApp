using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    public class UsersController : CustomBaseController //user ile ilgili işlemler yapılacağı zaman client tarafından gelen isteği route buraya yönlendirir
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper) //bağımlılıkların gevşetilmesi için dışarıdan enjekte edilmesi
        {
            _userService = userService; 
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() //bütün userların getirilmesi istenildiğinde çalışacak olan metod
        {
            var users = await _userService.GetAllAsync(); //asenkron olarak db'den bütün userların getirilip users değişkenine atanması
            var usersDto = _mapper.Map<List<UserDto>>(users.ToList()); //getirilmiş olan user listesi UserDto listesine dönüştürülür ve usersDto değişkenine atanır

            return CreateActionResult(CustomResponseDto<List<UserDto>>.Success(200, usersDto)); //UserDto listesi özel bir yanıt sisteöiyle CreateActionResult yardımcı metoduyla IActionResult türünde döndürülür ve http yanıtı olarak client'a gösterilir
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetSingleUserByIdWithCreditCardsAndUsersAsync(int userId) //userlar ile birlikte kredi kartlarının getirilmesini sağlayan metod
        {
            return CreateActionResult(await _userService.GetSingleUserByIdWithCreditCardsAndUsersAsync(userId));
        }
    }
}
