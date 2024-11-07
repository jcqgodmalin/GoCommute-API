using GoCommute;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using GoCommute.Repositories;
using GoCommute.Services;
using GoCommute.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

//Controllers
builder.Services.AddControllers();

//Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAppRepository, AppRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();

//Services
builder.Services.AddScoped<RouteService>();
builder.Services.AddScoped<CommuteService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AppService>();

//EF
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDBContext>(options => 
    options.UseSqlServer(connectionString)
);


//JWT
//UserService _userService = builder.Services.BuildServiceProvider().GetRequiredService<UserService>();
builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {

    options.Events = new JwtBearerEvents {

        OnMessageReceived  = async context => {

            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ","");

            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
            {
                context.Fail("Token is not in a valid JWT format.");
                return;
            }

            var jwtToken = handler.ReadJwtToken(token);

            var appIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "AppID")?.Value;
            if(appIdClaim == null){
                context.Fail("AppID is missing in the token");
                return;
            }

            var appService = context.HttpContext.RequestServices.GetRequiredService<AppService>();
            var app = await appService.GetAppByAppID(appIdClaim);
            if (string.IsNullOrEmpty(app.SecretKey))
            {
                context.Fail("Invalid AppID or SecretKey not found.");
                return;
            }

            // Revalidate the token using the retrieved secret key
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(app.SecretKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                context.Principal = new ClaimsPrincipal(handler.ValidateToken(token, validationParameters, out _));
                context.Success();
            }
            catch (Exception ex)
            {
                context.Fail($"Token validation failed: {ex.Message}");
            }
        },
        OnAuthenticationFailed = context => {
            context.Response.Headers.Append("Token-Error", context.Exception.Message);
            return Task.CompletedTask;
        }
    };
});

//Versioning
builder.Services.AddApiVersioning(options => {
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1,0);
    options.ReportApiVersions = true;
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();