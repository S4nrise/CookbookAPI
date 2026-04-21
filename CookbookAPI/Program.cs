using CookbookAPI.Abstractions;
using CookbookAPI.Configuration.Mapping;
using CookbookAPI.Extensions;
using CookbookAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IIngredientsRepository, IngredientsRepository>();
builder.Services.AddSingleton<IRecipesRepository, RecipesRepository>();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCustomExceptionHandler();

app.MapControllers();

app.Run();
