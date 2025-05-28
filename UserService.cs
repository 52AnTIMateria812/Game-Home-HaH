using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameHub.Models;
using GameHub.Repositories;
using GameHub.Validators;
using GameHub.Exceptions;

namespace GameHub.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserValidator _validator;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _validator = new UserValidator();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException($"Пользователь с ID {id} не найден");
            }
            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> CreateUserAsync(UserCreateDto userDto)
        {
            var validationResult = await _validator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Password = HashPassword(userDto.Password),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            return await _userRepository.AddAsync(user);
        }

        public async Task UpdateUserAsync(int id, UserUpdateDto userDto)
        {
            var user = await GetUserByIdAsync(id);
            
            if (!string.IsNullOrEmpty(userDto.Username))
                user.Username = userDto.Username;
            
            if (!string.IsNullOrEmpty(userDto.Email))
                user.Email = userDto.Email;
            
            if (!string.IsNullOrEmpty(userDto.Password))
                user.Password = HashPassword(userDto.Password);

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<GameSession>> GetUserSessionsAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            return await _userRepository.GetUserSessionsAsync(userId);
        }

        public async Task<IEnumerable<Friend>> GetUserFriendsAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            return await _userRepository.GetUserFriendsAsync(userId);
        }

        public async Task AddFriendAsync(int userId, int friendId)
        {
            var user = await GetUserByIdAsync(userId);
            var friend = await GetUserByIdAsync(friendId);
            
            if (await _userRepository.IsFriendAsync(userId, friendId))
            {
                throw new ValidationException("Пользователи уже являются друзьями");
            }

            await _userRepository.AddFriendAsync(userId, friendId);
        }

        public async Task RemoveFriendAsync(int userId, int friendId)
        {
            var user = await GetUserByIdAsync(userId);
            var friend = await GetUserByIdAsync(friendId);
            
            if (!await _userRepository.IsFriendAsync(userId, friendId))
            {
                throw new ValidationException("Пользователи не являются друзьями");
            }

            await _userRepository.RemoveFriendAsync(userId, friendId);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
