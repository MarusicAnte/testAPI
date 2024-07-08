using eStudent.Controllers;
using Microsoft.AspNetCore.Mvc;
using testAPI.Interfaces;
using testAPI.Models.Domain;
using testAPI.Models.DTO.UserDtos;
using testAPI.Query;

namespace testAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService) 
        {
            _userService = userService;        
        }


        [HttpGet]
        public async Task<List<UserDto>> GetAll([FromQuery] UserQuery userQuery)
        {
            return await _userService.GetAllUsers(userQuery);
        }


        [HttpGet("{id}")]
        public async Task<UserDto> GetById([FromRoute] int id)
        {
            return await _userService.GetUserById(id);
        }


        [HttpPost]
        public async Task<UserDto> Create([FromBody] CreateUserDto createUserDto)
        {
            return await _userService.CreateUser(createUserDto);
        }


        [HttpPatch("{id}")]
        public async Task<UserDto> Update([FromRoute] int id, [FromBody] UpdateUserDto updatedUserDto)
        {
            return await _userService.UpdateUserById(id, updatedUserDto);
        }

        [HttpDelete("{id}")]
        public async Task<UserDto> Delete([FromRoute] int id) 
        {
            return await _userService.DeleteUserById(id);
        }
    }
}
