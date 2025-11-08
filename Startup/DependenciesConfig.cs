using System.Text;
using BookStore.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.Startup;

public static class DependenciesConfig
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddCorsConfigServices();
        builder.Services.AddControllers();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApiServices();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["AppSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["AppSettings:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)
                    ),
                    ValidateIssuerSigningKey = true
                };
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddAutoMapper(typeof(Program).Assembly);

        builder.Services.AddDbContext<BookStoreContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlDbConnection"))
        );
        builder.Services.AddStackExchangeRedisCache(option =>
        {
            option.Configuration = builder.Configuration.GetConnectionString("Redis");
            option.InstanceName = "Books_";
        });

        builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
        builder.Services.AddScoped<IBookService, BookService>();
        builder.Services.AddScoped<IAuthService, AuthService>();

        builder.Services.AddAuthorization();
    }
}