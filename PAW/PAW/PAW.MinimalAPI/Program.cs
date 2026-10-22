using PAW.DataAccess.MSSQL;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;
using PawTask = PAW.Models.Task;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container (Dependency Injection / IoC)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext and TaskRepository for DI
builder.Services.AddDbContext<ProductDbContext>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Minimal API Group for Tasks using Dependency Injection
var taskGroup = app.MapGroup("/Task");

taskGroup.MapGet("/", async (ITaskRepository repo) =>
{
    var tasks = await repo.ReadAsync() ?? [];
    return Results.Ok(tasks.Select(TaskDTO.ConvertFrom));
})
.WithName("GetTasks");

taskGroup.MapGet("/{id:int}", async (int id, ITaskRepository repo) =>
{
    var task = await repo.FindAsync(id);
    return task is not null ? Results.Ok(TaskDTO.ConvertFrom(task)) : Results.NotFound();
})
.WithName("GetTaskById");

taskGroup.MapPost("/", async (PawTask task, ITaskRepository repo) =>
{
    var success = await repo.CreateAsync(task);
    return Results.Ok(success);
})
.WithName("CreateTask");

taskGroup.MapPut("/{id:int}", async (int id, PawTask task, ITaskRepository repo) =>
{
    task.Id = id;
    var success = await repo.UpdateAsync(task);
    return Results.Ok(success);
})
.WithName("UpdateTask");

taskGroup.MapDelete("/{id:int}", async (int id, ITaskRepository repo) =>
{
    var task = await repo.FindAsync(id);
    if (task == null) return Results.NotFound(false);
    var success = await repo.DeleteAsync(task);
    return Results.Ok(success);
})
.WithName("DeleteTask");

app.Run();

