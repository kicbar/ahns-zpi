var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5080");

builder.Services.AddCors(options =>
    options.AddPolicy("AngularClient", policy => policy
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()));

builder.Services.AddSingleton<StudentRepository>();

var app = builder.Build();
app.UseCors("AngularClient");

var students = app.MapGroup("/api/students");

students.MapGet("/", (StudentRepository repository) =>
    Results.Ok(repository.GetAll()));

students.MapGet("/{id:int}", (int id, StudentRepository repository) =>
{
    var student = repository.GetById(id);
    return student is null ? Results.NotFound() : Results.Ok(student);
});

students.MapPost("/", (StudentRequest request, StudentRepository repository) =>
{
    var student = repository.Add(request);
    return Results.Created($"/api/students/{student.Id}", student);
});

students.MapPut("/{id:int}", (int id, StudentRequest request, StudentRepository repository) =>
{
    var student = repository.Update(id, request);
    return student is null ? Results.NotFound() : Results.Ok(student);
});

students.MapDelete("/{id:int}", (int id, StudentRepository repository) =>
    repository.Delete(id) ? Results.NoContent() : Results.NotFound());

app.MapGet("/", () => Results.Ok(new { name = "Students API", endpoint = "/api/students" }));

app.Run();
