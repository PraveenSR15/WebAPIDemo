using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Data;

var builder = WebApplication.CreateBuilder(args);

//dbContext
builder.Services.AddDbContext<Repository>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShirtStoreManagement"));
});

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
