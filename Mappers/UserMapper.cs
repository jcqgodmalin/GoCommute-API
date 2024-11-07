using GoCommute.DTOs;

namespace GoCommute.Mappers
{
    public static class UserMapper
    {
        public static UserDto EntityToUserDto(User user) {
            return new UserDto {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
                Apps = user.Apps,
                Role = user.Role,
                Created_At = user.Created_At,
                Updated_At = user.Updated_At
            };
        }

        public static User UserDtoToEntity(UserDto userDto) {
            return new User {
                Id = userDto.Id,
                Email = userDto.Email,
                Password = userDto.Password,
                Apps = userDto.Apps,
                Role = userDto.Role,
                Created_At = userDto.Created_At,
                Updated_At = userDto.Updated_At
            };
        }
    }
}