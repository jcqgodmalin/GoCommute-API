using Azure.Core;
using GoCommute.DTOs;
using GoCommute.Helpers;
using GoCommute.Mappers;
using GoCommute.Models;
using GoCommute.Repositories;

namespace GoCommute.Services
{
    public class AppService
    {
        private readonly IAppRepository _appRepository;
        private readonly IUserRepository _userRepository;

        public AppService(IAppRepository appRepository, IUserRepository userRepository) {
            _appRepository = appRepository;
            _userRepository = userRepository;
        }

        public async Task<List<AppDto>> GetAllApps(){
            var apps = await _appRepository.GetAllApps();
            return apps.Select(AppMapper.EntityToAppDto).ToList();
        }

        public async Task<List<AppDto>> GetAppsByUserId(int id){
            var apps = await _appRepository.GetAllAppsByUserId(id);
            return apps.Select(AppMapper.EntityToAppDto).ToList();
        }

        public async Task<AppDto?> GetAppByUserId(int UserId, string appName){
            var app = await _appRepository.GetAppByUserId(UserId, appName);
            if(app == null){
                return null;
            }
            return AppMapper.EntityToAppDto(app);
        }

        public async Task<AppDto?> GetAppByID(int id){
            var app = await _appRepository.GetAppByID(id);
            if(app == null){
                return null;
            }
            return AppMapper.EntityToAppDto(app);
        }

        public async Task<AppDto?> GetAppByAppID(string AppID){
            var app = await _appRepository.GetAppByAppID(AppID);
            if(app == null){
                return null;
            }
            return AppMapper.EntityToAppDto(app);
        }

        public async Task<AppDto> AddApp(NewAppDto newAppDto){

            //check if user exists
            var user = await _userRepository.GetUser(newAppDto.UserId,null);
            if(user == null){
                throw new Exception("User does not exists");
            }

            //check if app is existing
            var existingApp = await _appRepository.GetAppByUserId(newAppDto.UserId, newAppDto.Name);
            if(existingApp != null){
                throw new Exception("App Name already exist for this user");
            }

            var newapp = new AppDto();

            newapp.Name = newAppDto.Name;
            newapp.UserId = newAppDto.UserId;
            newapp.SecretKey = SecurityHelper.GenerateSecretKey();
            newapp.AppID = SecurityHelper.GenerateAppID();
            newapp.Role = newAppDto.Role;
            newapp.Created_At = DateTime.Now;

            var createdApp = AppMapper.AppDtoToEntity(newapp);
            await _appRepository.AddApp(createdApp);
            Console.WriteLine($"New App Created: {createdApp.Id}");
            return AppMapper.EntityToAppDto(createdApp);
        }

        public async Task<bool> UpdateApp(AppDto updatedApp){
            return await _appRepository.UpdateApp(AppMapper.AppDtoToEntity(updatedApp));
        }

        public async Task<bool> DeleteApp(int id){
            return await _appRepository.DeleteApp(id);
        }

        public async Task<AppRefreshTokenDto> GenerateToken(AppGenerateTokenDto appGenerateTokenDto){

            //check if all fields are supplied correctly
            if(appGenerateTokenDto.AppID == null || appGenerateTokenDto.SecretKey == null){
                throw new Exception("AppID and SecretKey cannot be empty");
            }

            //get the app based on the provided AppID
            var app = await _appRepository.GetAppByAppID(appGenerateTokenDto.AppID);

            if(app == null){
                throw new Exception("App with the provided AppID and SecretKey cannot be found");
            }

            var atrt = new AppRefreshTokenDto();
            atrt.AccessToken = SecurityHelper.GenerateToken(AppMapper.EntityToAppDto(app));
            atrt.RefreshToken = Guid.NewGuid().ToString();
           

            //save the refresh token to app's record in db
            app.RefreshToken = atrt.RefreshToken;

            var saveRT = await _appRepository.UpdateApp(app);

            if(saveRT){
                return atrt;
            }else{
                throw new Exception("There was an error saving the refresh token");
            }

        }

        public async Task<AppRefreshTokenDto> RefreshToken(AppRefreshTokenDto appRefreshTokenDto){

            //validate if there is an app with the provided refreshToken
            var app = _appRepository.GetApp(a => a.RefreshToken == appRefreshTokenDto.RefreshToken)?.FirstOrDefault();

            if(app == null){
                throw new Exception("Refresh Token is invalid");
            }

            //get the principal of the expired token
            var principal = SecurityHelper.GetPrincipalFromExpiredToken(appRefreshTokenDto.AccessToken,app.SecretKey);
            if(principal == null){
                throw new Exception("Access Token is invalid");
            }

            //if principal is not null, generate the new access and refresh token
            var atrt = new AppRefreshTokenDto();
            atrt.AccessToken = SecurityHelper.GenerateToken(AppMapper.EntityToAppDto(app));
            atrt.RefreshToken = Guid.NewGuid().ToString();

            //save the new RefreshToken
            app.RefreshToken = atrt.RefreshToken;
            var updateApp = await _appRepository.UpdateApp(app);

            if(updateApp){
                return atrt;
            }else{
                throw new Exception("There was an error saving the new refresh token");
            }

        }
    }
}