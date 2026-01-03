using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// STEPS: 0. Add support for environment variables
builder.Configuration.AddEnvironmentVariables();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddHealthChecks();

var databaseConfigurationsExample = builder.Configuration.GetSection(("DatabaseConfigurations"));

//DatabaseConfigurations__Port
//DatabaseConfigurations__Host
//DatabaseConfigurations__Name
Console.WriteLine("Port: {0}",databaseConfigurationsExample.GetSection("Port").Value);
Console.WriteLine("Host: {0}",databaseConfigurationsExample.GetSection("Host").Value);
Console.WriteLine("Name: {0}",databaseConfigurationsExample.GetSection("Name").Value);

var apiKeyConfigurations = builder.Configuration.GetSection(("ApiConfigurations"));

//ApiConfigurations__Key
Console.WriteLine("ApiKey: {0}",apiKeyConfigurations.GetSection("Key").Value);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("../swagger/v1/swagger.json", "POC Grupo 118 - Fase 4 | Swagger");
    s.RoutePrefix = string.Empty;
    s.DocumentTitle = "POC Grupo 118 - Fase 4 | Swagger";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/healthz");
app.Run();