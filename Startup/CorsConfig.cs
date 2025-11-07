namespace BookStore.Startup;

public static class CorsConfig
{
    private const string AllowAllPolicy = "AllowAll";

    public static void AddCorsConfigServices(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(AllowAllPolicy, policy =>
            {
                policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
            });
        });
    }

    public static void UseCorsConfig(this WebApplication app)
    {
        app.UseCors(AllowAllPolicy);
    }
}