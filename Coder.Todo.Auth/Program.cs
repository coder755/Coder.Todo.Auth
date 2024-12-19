using System.Text.Json.Serialization;
using Coder.Todo.Auth.Db;
using Coder.Todo.Auth.Services.User;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Ensure appsettings.json is loaded
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

// Add services to the container
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var connectionString = GetConnectionString(builder.Configuration);

builder.Services.AddDbContext<AuthContext>(options =>
{
    var serverVersion = new MySqlServerVersion("8.0");
    options.UseMySql(connectionString, serverVersion);
});

builder.Services.AddScoped<IUserService, UserService>();

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

app.UseHttpsRedirection();
app.MapHealthChecks("/healthcheck");
app.MapControllers();
app.Run();


string GetConnectionString(IConfiguration configuration)
{
    const string dbSection = "Todo.Storage:Db";
    var dbConfig = configuration.GetSection(dbSection);
    var server = dbConfig.GetValue<string>("Server");
    var port = dbConfig.GetValue<string>("Port");
    var database = dbConfig.GetValue<string>("Db");
    var userId = Environment.GetEnvironmentVariable("TODO_DB_ID");
    var password = Environment.GetEnvironmentVariable("TODO_DB_PW");
    var connStr = $"server={server};port={port};user={userId};password={password};database={database};";

    return connStr;
}