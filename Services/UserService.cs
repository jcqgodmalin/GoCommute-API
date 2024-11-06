using GoCommute.DTOs;
using GoCommute.Mappers;
using GoCommute.Repositories;

namespace GoCommute.Services
{
    public class UserService
    {

        private readonly IUserReporitory _userRepository;

        public UserService(IUserReporitory userReporitory)
        {
            _userRepository = userReporitory;
        }

        public async Task<UserDto?> GetUser(int? id = null, string? email = null, string? appid = null, string? secretkey = null){
            var user = await _userRepository.GetUser(id, email, appid, secretkey);
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

        public async Task<string?> GetSecretKey(string AppID){
            var secretKey = await _userRepository.GetSecretKeyByAppID(AppID);
            if(secretKey != null){
                return secretKey;
            }
            return null;
        }
    }
}