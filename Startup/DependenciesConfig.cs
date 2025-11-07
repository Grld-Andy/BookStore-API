using BookStore.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Startup;

public static class DependenciesConfig
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddCorsConfigServices();
        builder.Services.AddControllers();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApiServices();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
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

        builder.Services.AddAuthorization();
    }
}