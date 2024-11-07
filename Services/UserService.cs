using GoCommute.DTOs;
using GoCommute.Mappers;
using GoCommute.Repositories;

namespace GoCommute.Services
{
    public class UserService
    {

        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto?> GetUser(int? id = null, string? email = null){
            var user = await _userRepository.GetUser(id, email);
            if(user != null){
                return UserMapper.EntityToUserDto(user);
            }
            return null;
        }

        public async Task<UserDto> AddUser(UserDto userDto){
            var user = UserMapper.UserDtoToEntity(userDto);
            await _userRepository.AddUser(user);
            return UserMapper.EntityToUserDto(user);
        }

        // public async Task<string?> GetSecretKey(string AppID){
        //     var secretKey = await _userRepository.GetSecretKeyByAppID(AppID);
        //     if(secretKey != null){
        //         return secretKey;
        //     }
        //     return null;
        // }
    }
}