using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOData(options =>
    options.Filter().Select().OrderBy());

var app = builder.Build();

var students = new List<Student>
{
    new(1, "John", "Smith", 22),
    new(2, "Anna", "Brown", 20),
    new(3, "Peter", "Wilson", 24)
};

app.MapGet("/odata/students", () => students.AsQueryable())
    .AddODataQueryEndpointFilter()
    .WithODataResult();

app.Run();

public record Student(int Id, string FirstName, string LastName, int Age);