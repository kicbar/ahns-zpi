var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => new
{
    Id = 1,
    FirstName = "John",
    LastName = "Smith",
    Age = 22
});

app.Run();