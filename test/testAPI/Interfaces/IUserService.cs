using Microsoft.AspNetCore.Mvc;
using testAPI.Models.Domain;
using testAPI.Models.DTO.UserDtos;
using testAPI.Query;

namespace testAPI.Interfaces
{
    public interface IUserService
    {
        public Task<List<UserDto>> GetAllUsers(UserQuery userQuery);
        public Task<UserDto> GetUserById(int id);
        public Task<UserDto> CreateUser(CreateUserDto createUserDto);
        public Task<UserDto> UpdateUserById(int id, UpdateUserDto updatedUserDto);
        public Task<UserDto> DeleteUserById(int id);
        Task<User> ValidateUserAsync(string email, string password);
    }
}
