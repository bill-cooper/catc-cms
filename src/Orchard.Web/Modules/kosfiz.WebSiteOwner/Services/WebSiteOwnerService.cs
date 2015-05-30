using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using kosfiz.WebSiteOwner.Models;
using Orchard.Data;
using Orchard.Caching;

namespace kosfiz.WebSiteOwner.Services
{
    public class WebSiteOwnerService: IWebSiteOwnerService
    {
        IRepository<WebSiteOwnerRecord> _repository;
        ISignals _isignals;
        public WebSiteOwnerService(IRepository<WebSiteOwnerRecord> repository, ISignals signals)
        {
            _isignals = signals;
            _repository = repository;
        }

        public WebSiteOwnerRecord Get(int Id)
        {
            return _repository.Table.Where(x => x.Id == Id).FirstOrDefault();
        }

        public List<WebSiteOwnerRecord> Get()
        {
            return _repository.Table.ToList();
        }

        public bool Set(int Id, string Title, string MetaName, string MetaContent)
        {
            bool result = false;
            var record = Get(Id);
            if (record != null)
            {
                record.Title = Title;
                record.MetaName = MetaName;
                record.MetaContent = MetaContent;
                _isignals.Trigger("kosfiz.WebSiteOwnerRecordChanged");
                return true;
            }

            return result;
        }

        public bool Delete(int Id)
        {
            bool result = false;
            var record = Get(Id);
            if (record != null)
            {
                _repository.Delete(record);
                _isignals.Trigger("kosfiz.WebSiteOwnerRecordChanged");
            }
            return result;
        }

        public void Add(string Title, string MetaName, string MetaContent)
        {
            _isignals.Trigger("kosfiz.WebSiteOwnerRecordChanged");
            _repository.Create(new WebSiteOwnerRecord { Title = Title, MetaName = MetaName, MetaContent = MetaContent });
        }
    }
}