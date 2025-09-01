using CQRS.Data.DAL;
using CQRS.DataAccess;
using CQRS.DataAccess.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IMongoDb, MongoDb>();
builder.Services.AddScoped<IMediator, Mediator>();

MediatorHandler.RegisterHandlers(builder.Services);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();