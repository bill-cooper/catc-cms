using ceenq.com.Core.Accounts;
using ceenq.com.Core.Models;
using Orchard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ceenq.com.Core.Extensions
{
    public static class WorkContextExtensions
    {
        public static IAccount GetAccount(this WorkContext workContext)
        {


            //if (workContext.GetState<IAccount>("Account") == null)
            //{
            //    var url = workContext.HttpContext.Request.Url.AbsolutePath;
            //    var urlParts = url.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            //    var accountUrlPart = urlParts[1];

            //    var account = _accountService.GetAccountByName(accountUrlPart);

            //    if (_accountService.GetAccountByName(accountUrlPart) == null)
            //        return false;
            //}
            //workContext.SetState<string>("Account", account);

            return workContext.GetState<IAccount>("Account");
        }
        public static void SetAccount(this WorkContext workContext, string account)
        {
            workContext.SetState<string>("Account", account);
        }
    }
}