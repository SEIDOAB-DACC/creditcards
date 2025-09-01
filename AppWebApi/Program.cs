using Configuration.Options;
using DbContext;
using DbRepos;
using Microsoft.EntityFrameworkCore;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddEndpointsApiExplorer();

var currentDir = Directory.GetCurrentDirectory();
var assembly = System.Reflection.Assembly.Load("Configuration");
builder.Configuration.SetBasePath(Path.Combine(currentDir, "../AppWebApi"))
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddUserSecrets(assembly);

builder.Services.Configure<AesEncryptionOptions>(
    options => builder.Configuration.GetSection(AesEncryptionOptions.Position).Bind(options));

// adding verion info
builder.Services.Configure<VersionOptions>(options => VersionOptions.ReadFromAssembly(options));


// adding DbContexts
builder.Services.AddDbContext<MainDbContext>(options =>
{
    // SQLSERVER
    var connectionString = builder.Configuration["ConnectionStrings:SqlServerDocker"];  //alternative to below
    //var connectionString = builder.Configuration.GetConnectionString("SqlServerDocker");
    options.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Seido Friends API",
#if DEBUG
        Version = "v2.0 DEBUG",
#else
        Version = "v2.0",
#endif
        Description = "This is an API used in Seido's various software developer training courses."
    });
});

//Inject DbRepos and Services
builder.Services.AddScoped<AdminDbRepos>();

builder.Services.AddScoped<IAdminService, AdminServiceDb>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Seido Friends API v2.0");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
