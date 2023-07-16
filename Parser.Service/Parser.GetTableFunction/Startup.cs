using Parser.Business.Parser.Manager;
using Parser.Common.TableManager;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using Parser.Common.SqlRepository;
using Parser.Repos.SQLConnect.Repositories;
using Parser.Utils.Shared;
using Microsoft.Extensions.Configuration;
using Utils.Shared;

[assembly: FunctionsStartup(typeof(Parser.GetTableFunction.Startup))]
namespace Parser.GetTableFunction
{
    public class Startup : FunctionsStartup
    {
        private const string _keyVaultUrl = "https://key-vault-panfilenko.vault.azure.net/";
        public override void Configure(IFunctionsHostBuilder builder)
        {
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
                new Uri(_keyVaultUrl),
                new DefaultAzureCredential(),
                options);

            var configs = new ConfigurationBuilder().AddEnvironmentVariables().Build();

            configs["DatabaseConnectionString"] = secretClient.GetSecret("DatabaseConnectionString").Value.Value;
            configs["TelegramChatIdentifier"] = secretClient.GetSecret("TelegramChatIdentifier").Value.Value;
            configs["TelegramIdentifier"] = secretClient.GetSecret("TelegramIdentifier").Value.Value;

            builder.Services.Configure<FunctionSettings>(configs);

            builder.Services.AddAutoMapper(typeof(TableInfoRepository));
            builder.Services.AddSingleton<ITableInfoManager, TableInfoManager>();
            builder.Services.AddSingleton<ITableInfoRepository, TableInfoRepository>();
        }
    }
}
