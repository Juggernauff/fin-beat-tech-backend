using Asp.Versioning;
using FinBeat.API.Middlewares;
using FinBeat.DAL.Repositories;
using FinBeat.DAL.Repositories.Implementation;
using FinBeat.Services.Mappings;
using FinBeat.Services.Services;
using FinBeat.Services.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy
        .WithOrigins("http://localhost:3000")
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
builder.Services.AddScoped<IEntityRepository>(provider => new EntityRepository(connectionString));
builder.Services.AddScoped<IEntityService, EntityService>();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseCors("AllowSpecificOrigin");

using (var scope = app.Services.CreateScope())
{
    var repository = scope.ServiceProvider.GetRequiredService<IEntityRepository>();
    await repository.EnsureTableExistsAsync();
}

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
