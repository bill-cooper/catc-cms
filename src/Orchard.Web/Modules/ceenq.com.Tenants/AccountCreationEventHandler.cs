using ceenq.com.Core.Accounts;
using ceenq.com.Core.Infrastructure.Database;
using ceenq.com.Core.Tenants;

namespace ceenq.com.Tenants
{
    public class AccountCreationEventHandler : IAccountCreationEventHandler
    {
        private readonly ITenantService _tenantService;
        private readonly ITenantRecipe _tenantRecipe;
        private readonly IDatabaseManagement _databaseManagement;
        public AccountCreationEventHandler(ITenantService tenantService, ITenantRecipe tenantRecipe, IDatabaseManagement databaseManagement)
        {
            _tenantService = tenantService;
            _tenantRecipe = tenantRecipe;
            _databaseManagement = databaseManagement;
        }

        public void Initialize(AccountCreationEventContext context)
        {
        }

        public void Created(AccountCreationEventContext context)
        {
            //create database
            var databaseInfo = _databaseManagement.Create(new DatabaseCreateParameters() { Account = context.Account });

            var recipe = _tenantRecipe.Build(context.Account.Name);
            //create orchard tenant for the account
            _tenantService.Setup(
                ////REFACTOR - do not hard code the admin username and password.
                new TenantCreationContext()
                {
                    SiteName = context.Account.Name,
                    RequestUrlHost = context.Account.Domain,
                    AdminUsername = "admin",
                    AdminPassword = "test1234",
                    DatabaseProvider = "SqlServer",
                    DatabaseConnectionString = databaseInfo.ConnectionString,
                    DatabaseTablePrefix = "",
                    EnabledFeatures = { },
                    Recipe = recipe
                });
        }

        public void PostCreation(AccountCreationEventContext context)
        {
        }
    }
}