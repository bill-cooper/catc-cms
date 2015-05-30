using ceenq.com.Core.Accounts;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Assets;
using ceenq.com.Core.Models;
using ceenq.com.Core.Tenants;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Email.Models;
using Orchard.Environment.Extensions;
using Orchard.Recipes.Models;
using Orchard.Recipes.Services;

namespace ceenq.com.Tenants.Services
{

    [OrchardSuppressDependency("ceenq.com.Core.Tenants.DefaultTenantRecipe")]
    public class TenantRecipe : ITenantRecipe
    {
        private readonly IRecipeParser _recipeParser;
        private readonly IOrchardServices _orchardServices;
        private readonly IAssetStorageCredentialsProvider _assetStorageCredentials;
        private readonly IAccountContext _accountContext;
        public TenantRecipe(IRecipeParser recipeParser, IOrchardServices orchardServices, IAssetStorageCredentialsProvider assetStorageCredentials, IAccountContext accountContext)
        {
            _recipeParser = recipeParser;
            _orchardServices = orchardServices;
            _assetStorageCredentials = assetStorageCredentials;
            _accountContext = accountContext;
        }

        public Recipe Build(string name)
        {
            //get the account domain set up on the main host instance (the cnq system).  Since this module
            //will only be enabled for the core system, this should provide a domain that will act as a top
            //level domain for all clients
            var parentDomain = _orchardServices.WorkContext.CurrentSite.As<CoreSettingsPart>().AccountDomain;
            var accountDomain = string.Format("{0}.{1}", name, parentDomain);
            var baseUrl = string.Format("https://{0}", accountDomain);
                

            var smtpSetting = _orchardServices.WorkContext.CurrentSite.As<SmtpSettingsPart>();

            string recipe = string.Format(
          @"<?xml version=""1.0""?>
            <Orchard>
              <Recipe>
                <Name>Cnq Tenant</Name>
              </Recipe>
              <Feature enable=""
                        ,Orchard.Alias
                        ,Orchard.Autoroute
                        ,Orchard.jQuery
                        ,Orchard.ContentTypes
                        ,Orchard.Scripting
                        ,Orchard.Scripting.Lightweight
                        ,Orchard.Packaging
                        ,Orchard.OutputCache
                        ,Orchard.ContentPermissions
                        ,Orchard.ImportExport
                        ,Orchard.Indexing
                        ,Orchard.Search
                        ,Orchard.JobsQueue
                        ,Orchard.Email
                        ,Lucene
                        
                        ,ceenq.com.Core
                        ,ceenq.com.Apps
                        ,ceenq.com.AppRoutingServer
                        ,ceenq.com.CloudAssetManagement
                        ,ceenq.com.AzureManagement
                        ,ceenq.com.AwsManagement
                        ,ceenq.com.SshClient
                        ,ceenq.com.LinuxCommands
                        ,ceenq.com.Common
                        ,ceenq.com.AssetImport
                        ,ceenq.com.Assets
                        ,ceenq.com.RoutingServer
                        ,ceenq.com.Users
                        ,ceenq.com.ApplicationAPI
                        ,ceenq.com.ManagementAPI
                        ,ceenq.com.DashboardApp

                        ,Orchard.Blogs
                        ,ceenq.com.Layouts
                        ,Orchard.Layouts.Projections
                        ,ceenq.com.Workflow
                        ,ceenq.com.AuditTrail
                        ,Orchard.Fields
                        ,Orchard.DynamicForms
                        ,TinyMce
                        ,ceenq.com.Theme.LayoutsAdmin
                        ,ceenq.com.Theme.Layouts

                       ""/>
              <Metadata>
                <Types>
                  <Page ContentTypeSettings.Draftable=""True"" />
                </Types>
                <Parts>
                  <BodyPart BodyPartSettings.FlavorDefault=""html"" />
                </Parts>
              </Metadata>

              <Settings>
                <RegistrationSettingsPart UsersCanRegister=""false"" UsersMustValidateEmail=""true"" ValidateEmailContactEMail=""info@ceenq.com"" UsersAreModerated=""false"" NotifyModeration=""false"" NotificationsRecipients="""" EnableLostPassword=""false""/>
                <CoreSettingsPart AccountDomain=""{1}"" ParentTenant=""{13}""/>
                <CloudStorageSettingsPart StorageAccount=""{2}"" StorageKey=""{3}"" />
                <SmtpSettingsPart Address=""{4}"" Host=""{5}"" Port=""{6}"" EnableSsl=""{7}"" RequireCredentials=""{8}"" UserName=""{9}"" Password=""{10}""/>
                <ClientIpAddressSettingsPart EnableClientIpAddressHeader=""true"" ClientIpAddressHeaderName=""X-Real-IP"" />
                <AuditTrailSettingsPart EnableClientIpAddressLogging=""true"" />
              </Settings>
              <Roles>
	              <Role Permissions="""" Name=""ApplicationUser""/>
	              <Role Permissions=""EditOwnContent,EditContent,PublishOwnContent,PublishContent,DeleteOwnContent,DeleteContent,ViewContent,ViewOwnContent,ManageUsers,AssignRoles,AssignApplications,ManageWorkflows,ManageQueries,ViewAuditTrail,ManageAuditTrailSettings,ImportAuditTrail,ManageClientIpAddressSettings,ViewContentTypes,EditContentTypes,ManageOwnBlogs,ManageBlogs,EditOwnBlogPost,EditBlogPost,PublishOwnBlogPost,PublishBlogPost,DeleteOwnBlogPost,DeleteBlogPost,GrantPermission,GrantOwnPermission,AccessAdminPanel"" Name=""AccountAdmin""/>
              </Roles>
              <Migration features=""*"" />

              <Command>
                site setting set baseurl /BaseUrl:""{0}"" /Force:true 
                user create /UserName:""{11}"" /Password:""{12}""
                user add role {11} AccountAdmin
                routingserver create /DefaultDomain:""{1}""
                theme activate ""ceenq.com.Theme.Layouts""
              </Command>
            </Orchard>
            
            ", baseUrl
             ,accountDomain
             ,_assetStorageCredentials.Username
             ,_assetStorageCredentials.Password
             ,smtpSetting.Address
             ,smtpSetting.Host
             ,smtpSetting.Port
             ,smtpSetting.EnableSsl
             ,smtpSetting.RequireCredentials
             ,smtpSetting.UserName
             ,smtpSetting.Password
             ,name
             ,"test1234"
             ,_accountContext.Account
             );

            return _recipeParser.ParseRecipe(recipe);
        }
    }
}