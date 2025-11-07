using Scalar.AspNetCore;

namespace BookStore.Startup;

public static class OpenApiConfig
{
    public static void AddOpenApiServices(this IServiceCollection services)
    {
        services.AddOpenApi();
    }

    public static void UseOpenApiServices(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.MapScalarApiReference(options =>
            {
                options.Title = "BookStore API";
                options.Theme = ScalarTheme.Saturn;
                options.Layout = ScalarLayout.Modern;
            });
        }
    }

}