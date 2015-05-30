using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.UI.Admin;
using kosfiz.WebSiteOwner.Services;
using Orchard.Localization;
using Orchard;
using kosfiz.WebSiteOwner.ViewModels;

namespace kosfiz.WebSiteOwner.Controllers
{
    [ValidateInput(false), Admin]
    public class AdminController: Controller
    {
        private readonly IWebSiteOwnerService _webSiteOwnerService;

        public IOrchardServices orchardServices { get; set; }
        public Localizer T { get; set; }

        public AdminController(IWebSiteOwnerService webSiteOwnerService, IOrchardServices Service)
        {
            orchardServices = Service;
            _webSiteOwnerService = webSiteOwnerService;
            T = NullLocalizer.Instance;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var listOfRecords = _webSiteOwnerService.Get();
            if (listOfRecords == null || listOfRecords.Count == 0)
                ViewBag.EmptyMessage = T("No data");
            return View(listOfRecords);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateEditWebSiteOwnerModel model)
        {
            if (!ModelState.IsValid)
                return View();
            _webSiteOwnerService.Add(model.Title, model.MetaName, model.MetaContent);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            return View(_webSiteOwnerService.Get(Id));
        }

        [HttpPost]
        public ActionResult Edit(int Id, CreateEditWebSiteOwnerModel model)
        {
            if(!ModelState.IsValid)
                return View(_webSiteOwnerService.Get(Id));
            _webSiteOwnerService.Set(Id, model.Title, model.MetaName, model.MetaContent);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            _webSiteOwnerService.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}