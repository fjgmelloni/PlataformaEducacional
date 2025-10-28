using PlataformaEducacional.Api.Configurations;
using PlataformaEducacional.Configurations;

var builder = WebApplication.CreateBuilder(args);


builder.Host.ConfigureAppSettings();

builder.Services
    .AddApiConfig()
    .AddCorsConfig(builder.Configuration)
    .AddSwaggerConfiguration()
    .AddDbContextConfiguration(builder.Configuration)
    .AddIdentityConfiguration()
    .RegisterServices()
    .AddJwtConfiguration(builder.Configuration);

var app = builder.Build();

app.UseSwaggerConfiguration();

app.UseApiConfiguration(app.Environment);

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.UseDbMigrationHelper();

app.Run();

public partial class Program { }
