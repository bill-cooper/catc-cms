using System;
using System.Security.Cryptography.X509Certificates;
using ceenq.com.AzureManagement.Models;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Infrastructure.Database;
using Microsoft.Azure;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Sql;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.UI.Notify;

namespace ceenq.com.AzureManagement.Services
{
    [OrchardSuppressDependency("ceenq.com.Core.Infrastructure.Database.DefaultDatabaseManagement")]
    public class SqlServerManagement :Component, IDatabaseManagement
    {
        private readonly INotifier _notifier;
        public SqlServerManagement(INotifier notifier)
        {
            _notifier = notifier;
        }

        public DatabaseInfo Create(DatabaseCreateParameters parameters)
        {
            var account = parameters.Account;
            var azureSettings = account.As<AzureSettingsPart>();
            var cloudCredentials = new CertificateCloudCredentials(azureSettings.SubscriptionId, new X509Certificate2(Convert.FromBase64String(azureSettings.Base64EncodedCertificate)));
            var sqlManagementClient = new SqlManagementClient(cloudCredentials);

            try
            {
                sqlManagementClient.Databases.Create(azureSettings.SqlServerDatabaseName,
                    new Microsoft.WindowsAzure.Management.Sql.Models.DatabaseCreateParameters()
                    {
                        CollationName = azureSettings.SqlServerDatabaseCollation,
                        Edition = azureSettings.SqlServerDatabaseEdition,
                        MaximumDatabaseSizeInGB = azureSettings.SqlServerDatabaseMaximumSizeInGb,
                        Name = account.Name
                    });
            }
            catch (Exception ex)
            {
                _notifier.Error(T("Sql Server Database could not be created.  {0}",ex.Message));
                throw new OrchardFatalException(T("Sql Server Database could not be created.  {0}", ex.Message),ex);
            }

            var connectionString =
                string.Format(
                    "Server=tcp:{0}.{2},{3};Database={1};User ID={4}@{0};Password={5};Trusted_Connection=False;Encrypt=True;Connection Timeout={6};"
                    , azureSettings.SqlServerDatabaseName //server name
                    , account.Name
                    , azureSettings.SqlServerDomain
                    , azureSettings.SqlServerPort
                    , azureSettings.SqlServerDatabaseUsername
                    , azureSettings.SqlServerDatabasePassword
                    , azureSettings.SqlServerDatabaseConnectionTimeout);


            return new DatabaseInfo {ConnectionString = connectionString};
        }


    }
}