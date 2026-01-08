using FlyMaps.Configuration;
using FlyMaps.Data;
using FlyMaps.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//EF DbContext
builder.Services.AddDbContext<BioDataDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Services registration
builder.Services.AddScoped<IBioDataImporter, BioDataImporter>();
builder.Services.AddScoped<IBioDataService, BioDataService>();

//Configuration section registration
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

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

//app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
