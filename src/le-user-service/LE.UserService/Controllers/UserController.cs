using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Models.Requests;
using LE.UserService.Models.Responses;
using LE.UserService.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Controllers
{
    [Route("api/users")]
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
        public async Task<IActionResult> FillInfor(Guid id, BasicInfoRequest request, CancellationToken cancellationToken = default)
        {
            var userDto = _mapper.Map<UserDto>(request);
            userDto.FirstName = request.UserInfo?.FirstName;
            userDto.LastName = request.UserInfo?.LastName;
            userDto.MiddleName = request.UserInfo?.MiddleName;
            userDto.Introduction = request.UserInfo?.Introduction;
            userDto.Gender = request.UserInfo?.Gender;
            userDto.Country = request.UserInfo?.Country;
            userDto.Hobbies = request.UserInfo?.Hobbies;

            await _userService.SetBasicInfor(id, userDto, cancellationToken);
            return Ok();
        }

        [HttpGet("{id}/basic-information")]
        public async Task<IActionResult> GetBasicInfor(Guid id, CancellationToken cancellationToken = default)
        {
            var response = await _userService.GetUser(id, cancellationToken);
            return Ok(response);
        }

        [HttpGet("{id}/languages")]
        public async Task<IActionResult> GetUserLanguages(Guid id, CancellationToken cancellationToken = default)
        {
            var dtos = await _userService.GetUserLanguages(id, cancellationToken);
            var response = _mapper.Map<List<LangResponse>>(dtos);
            return Ok(response);
        }

        [HttpPost("{id}/change-avatar")]
        public async Task<IActionResult> ChangeAvatar(Guid id, string avatar, CancellationToken cancellationToken = default)
        {
            await _userService.ChangeAvatar(id, avatar, cancellationToken);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken = default)
        {
            var dtos = await _userService.GetUsers(cancellationToken);
            return Ok(dtos);
        }
    }
}


