using GoCommute.DTOs;
using GoCommute.Models;

namespace GoCommute.Mappers
{
    public class AppMapper
    {
        public static AppDto EntityToAppDto(App app) {
            return new AppDto {
                Id = app.Id,
                UserId = app.UserId,
                Name = app.Name,
                AppID = app.AppID,
                SecretKey = app.SecretKey,
                RefreshToken = app.RefreshToken,
                Role = app.Role,
                Created_At = app.Created_At,
                Updated_At = app.Updated_At
            };
        }

        public static App AppDtoToEntity(AppDto appDto) {
            return new App {
                Id = appDto.Id,
                UserId = appDto.UserId,
                Name = appDto.Name,
                AppID = appDto.AppID,
                SecretKey = appDto.SecretKey,
                RefreshToken = appDto.RefreshToken,
                Role = appDto.Role,
                Created_At = appDto.Created_At,
                Updated_At = appDto.Updated_At
            };
        }
    }
}