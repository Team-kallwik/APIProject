using DapperConfiguration.Repository;
using DapperConfiguration.Services; // <-- Add this line

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register repository and service layers
builder.Services.AddScoped<IEmploRepository, EmploRepository>();
builder.Services.AddScoped<IEmploService, EmploService>(); // <-- Add this line

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();