using API.Middleware;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
//Adding genric service to our project
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddCors(); //config our angular

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionsMiddleware>();

app.UseCors(x => x.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:4200", "https://localhost:4200"));

app.MapControllers();

//config our db to seed data automatically without using the dotnet tools            

  try
     {
      using var scope = app.Services.CreateScope(); //Creates a new IServiceScope that can be used to resolve scoped services.
      var services = scope.ServiceProvider;
               
     //Returns a service object of type StoreContext.
      var context = services.GetRequiredService<StoreContext>();
               
      await context.Database.MigrateAsync(); //this line create db if it doesn't exist and seed data if there is pending one
      await StoreContextSeed.SeedAsync(context);
    }
      catch (Exception ex)
    {
                
      var logger  = app.Services.GetRequiredService<ILogger<Program>>();
      logger.LogError(ex, "An error occured during migration");
    }

app.Run();
