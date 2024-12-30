using System.Text.Json.Serialization;
using Coder.Todo.Auth.Db;
using Coder.Todo.Auth.Model.Auth;
using Coder.Todo.Auth.Services.Authorization;
using Coder.Todo.Auth.Services.Authorization.Jwt;
using Coder.Todo.Auth.Services.Authorization.Permission;
using Coder.Todo.Auth.Services.Authorization.Role;
using Coder.Todo.Auth.Services.User;
using Coder.Todo.Auth.Util;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Ensure appsettings.json is loaded
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug(); // Optional for Debug output

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthSwaggerGen();
builder.Services.AddHealthChecks();


builder.Services.AddDbContext<AuthContext>(options =>
{
    var connectionString = ProgramSetupHelpers.GetConnectionString(builder.Configuration);
    var serverVersion = new MySqlServerVersion("8.0");
    options.UseMySql(connectionString, serverVersion);
});

builder.Services.AddHttpContextAccessor();
var userJwtOptions = builder.Configuration.GetSection("UserJwtOptions").Get<JwtOptions>();
if (userJwtOptions == null)
{
    throw new InvalidOperationException("UserJwtOptions not found in configuration");
}
builder.Services.AddSingleton(userJwtOptions);
builder.Services.AddJwtAuthentication("User", userJwtOptions);
builder.Services.AddAuthorization();

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthHeaderProvider, AuthHeaderProvider>();

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddMvc(options => options.EnableEndpointRouting = false)
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    })
    .AddNewtonsoftJson(options =>
    {
        options.AllowInputFormatterExceptionMessages = false;
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapHealthChecks("/healthcheck");
app.MapControllers();

app.Run();