using Asp.Versioning;
using FinBeat.API.Middlewares;
using FinBeat.DAL;
using FinBeat.DAL.Extensions;
using FinBeat.DAL.Services;
using FinBeat.Services.Mappings;
using FinBeat.Services.Services;
using FinBeat.Services.Services.Implementation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("X-Api-Version"));
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

var connectionString = builder.Configuration.GetConnectionString("PostgreSQL");

builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString, name: "PostgreSQL");

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services
    .AddDal(builder.Configuration)
    .AddRepositories();
builder.Services.AddScoped<IEntityService, EntityService>();

var app = builder.Build();

var dbInitializer = app.Services.GetRequiredService<IDatabaseInitializer>();
await dbInitializer.InitializeAsync();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseCors("AllowSpecificOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
