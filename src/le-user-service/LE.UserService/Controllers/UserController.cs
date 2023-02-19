using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Models.Requests;
using LE.UserService.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LE.UserService.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("{id}/fill-basic-information")]
        public async Task<IActionResult> FillInfor(Guid id, BasicInfoRequest request)
        {
            var userDto = _mapper.Map<UserDto>(request);
            userDto.FirstName = request.UserInfo?.FirstName;
            userDto.LastName = request.UserInfo?.LastName;
            userDto.MiddleName = request.UserInfo?.MiddleName;
            userDto.Introduction = request.UserInfo?.Introduction;
            userDto.Gender = request.UserInfo?.Gender;

            await _userService.SetBasicInfor(id, userDto);
            return Ok();
        }

        [HttpGet("{id}/basic-information")]
        public async Task<IActionResult> GetBasicInfor(Guid id)
        {
            var response = await _userService.GetUser(id);
            return Ok(response);
        }
    }
}
