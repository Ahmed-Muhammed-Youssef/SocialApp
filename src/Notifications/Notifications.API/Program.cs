var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms
builder.Services.AddOpenApi();
builder.AddDatabase();

var app = builder.Build();

await DatabaseInitializer.InitializeAsync(app);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/push", () =>
{
    return TypedResults.Ok();
})
.WithName("push");

await app.RunAsync();
