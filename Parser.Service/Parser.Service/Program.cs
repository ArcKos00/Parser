using Parser.Business.Parser.Manager;
using Parser.Common.BlobRepository;
using Parser.Common.Repos.Parser;
using Parser.Common.Filters;
using Parser.Common.Parser.Blob;
using Parser.Common.ParserManager;
using Parser.Common.SqlManager;
using Parser.Common.SqlRepository;
using Parser.Repos.Blob;
using Parser.Repos.SQLConnect.Repositories;
using Parser.Utils.Shared;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

var builder = WebApplication.CreateBuilder(args);
var configs = GetConfig();

var options = new SecretClientOptions()
{
    Retry =
    {
        Delay = TimeSpan.FromSeconds(2),
        MaxDelay = TimeSpan.FromSeconds(16),
        MaxRetries = 5,
        Mode = RetryMode.Exponential
    }
};

SecretClient secretClient = new SecretClient(
    new Uri(configs["KeyVaultUrl"] ?? throw new ArgumentException()),
    new DefaultAzureCredential(),
    options);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ExceptionFilter));
}).AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

configs["AzureContainerUrl"] = secretClient.GetSecret("AzureContainerUrl").Value.Value;
configs["BlobConnectionString"] = secretClient.GetSecret("BlobConnectionString").Value.Value;
configs["BlobContainerName"] = secretClient.GetSecret("BlobContainerName").Value.Value;
configs["DatabaseConnectionString"] = secretClient.GetSecret("DatabaseConnectionString").Value.Value;

builder.Services.Configure<AppSettings>(configs);
builder.Services.AddAutoMapper(typeof(Parser.Repos.Parser.Parser), typeof(CarRepository), typeof(Program));

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient();

builder.Services.AddSingleton<IParser, Parser.Repos.Parser.Parser>();
builder.Services.AddSingleton<IBlob, Blob>();

builder.Services.AddSingleton<IImageRepository, ImageRepository>();
builder.Services.AddSingleton<IBlobRepository, BlobRepository>();
builder.Services.AddSingleton<ICarRepository, CarRepository>();
builder.Services.AddSingleton<IComplectationRepository, ComplectationRepository>();
builder.Services.AddSingleton<IGroupRepository, GroupRepository>();
builder.Services.AddSingleton<ISubGroupRepository, SubGroupRepository>();
builder.Services.AddSingleton<IDetailRepository, DetailRepository>();
builder.Services.AddSingleton<IBrandRepository, BrandRepository>();

builder.Services.AddSingleton<ISqlManager, SqlManager>();
builder.Services.AddSingleton<IGetSqlManager, SqlManager>();
builder.Services.AddSingleton<IParserManager, ParserManager>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

IConfiguration GetConfig()
{
    return new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();
}