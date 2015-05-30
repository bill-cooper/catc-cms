using System;
using System.Linq;
using System.Web.Mvc;
using ceenq.com.Logging.ViewModel;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Orchard.Localization;
using Orchard.Logging;

namespace ceenq.com.Logging.Controllers
{
    public class AdminController : Controller
    {
        public AdminController()
        {
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }
        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public ActionResult Log()
        {
       
            var tableClient = new CloudTableClient(
    new StorageUri(new System.Uri("https://cnqcmsstore.table.core.windows.net/")),
    new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
        "cnqcmsstore",
        "NpGJuTPbwIzr4vIB2oEPT7Ip2sEzrrlKJfGTFsErcZu/GqcbWdwIfmT+92iuEpNVO4g+Z48ht9yeoupL1psP3Q=="
        ));



            CloudTable table = tableClient.GetTableReference("WADLogsTable");


            // Read storage
            var query =
               new TableQuery()
                  .Where(
                  TableQuery.GenerateFilterConditionForDate(
                    "Timestamp",
                     QueryComparisons.GreaterThanOrEqual, DateTime.Now.AddMinutes(-30)));
            var list = table.ExecuteQuery(query).ToList();
            var modelList = list
                .OrderByDescending(item => item.Timestamp).Select(item => new LogViewModel { Timestamp = item.Timestamp.AddHours(-5).ToString(), Message = item.Properties["Message"].StringValue, Level = item.Properties["Level"].Int32Value })
                .Where(item => !item.Message.Contains("NHibernate.Cache.ReadWriteCache - An item was expired by the cache while it was locked") && !item.Message.Contains("DeadServerCallback"))
                .Distinct(new LogEntryCompare())
                .ToList();
            return View(modelList);
        }


    }
}