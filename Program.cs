global using BookStore.Models;
global using BookStore.Services;
using BookStore.Middlewares;
using BookStore.Startup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/openapi/v1.json", "My API v1");
    });
}

app.UseHttpsRedirection();
app.UseCorsConfig();

app.UseAuthorization();

app.UseMiddleware<GlobalErrorHandler>();

app.MapControllers();

app.Run();
