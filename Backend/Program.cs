using Backend.Data;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
 .AddJsonOptions(options =>
  {
      options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
      options.JsonSerializerOptions.WriteIndented = true;
      options.JsonSerializerOptions.MaxDepth = 64; // Optional: increase max depth
      options.JsonSerializerOptions.PropertyNamingPolicy = null;
  });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Pay attention!
// Add EF DbContext
builder.Services.AddDbContext<BioDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register CsvImporter
builder.Services.AddScoped<ICsvImporter, CsvImporter>();
builder.Services.AddScoped<IBioinformaticsService, BioinformaticsService>();
builder.Services.AddScoped<IElasticService, ElasticService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Auto-migrate database on startup (optional - remove in production)
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<BioDbContext>();
//    try
//    {
//        context.Database.Migrate();
//    }
//    catch (Exception ex)
//    {
//        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
//        logger.LogError(ex, "An error occurred while migrating the database");
//    }
//}

app.Run();