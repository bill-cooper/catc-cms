﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using Orchard.Indexing;
using Orchard.Localization;
using Orchard.Search.Models;
using Orchard.Search.ViewModels;

namespace Orchard.Search.Drivers {

    [OrchardFeature("Orchard.Search.Content")]
    public class AdminSearchSettingsPartDriver : ContentPartDriver<AdminSearchSettingsPart> {
        private readonly IIndexManager _indexManager;

        public AdminSearchSettingsPartDriver(IIndexManager indexManager) {
            _indexManager = indexManager;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override string Prefix { get { return "AdminSearchSettings"; } }

        protected override DriverResult Editor(AdminSearchSettingsPart part, dynamic shapeHelper) {
            return Editor(part, null, shapeHelper);
            
        }

        protected override DriverResult Editor(AdminSearchSettingsPart part, IUpdateModel updater, dynamic shapeHelper) {
            return ContentShape("Parts_AdminSearch_SiteSettings", () => {
                var model = new SearchSettingsViewModel();
                String[] searchedFields = part.SearchedFields;

                if (updater != null) {
                    // submitting: rebuild model from form data
                    if (updater.TryUpdateModel(model, Prefix, null, null)) {
                        // update part if successful
                        part.SearchIndex = model.SelectedIndex;
                        part.SearchedFields = model.Entries.First(e => e.Index == model.SelectedIndex).Fields.Where(e => e.Selected).Select(e => e.Field).ToArray();
                        part.FilterCulture = model.FilterCulture;
                    }
                }
                else if (_indexManager.HasIndexProvider()) {
                    // viewing editor: build model from part
                    model.FilterCulture = part.FilterCulture;
                    model.SelectedIndex = part.SearchIndex;
                    model.Entries = _indexManager.GetSearchIndexProvider().List().Select(x => {
                        var indexSettings = new IndexSettingsEntry {
                            Index = x,
                            Fields = new List<SearchSettingsEntry>()
                        };
                        foreach (var field in _indexManager.GetSearchIndexProvider().GetFields(x)) {
                            indexSettings.Fields.Add(new SearchSettingsEntry {Field = field, Selected = (x == part.SearchIndex && searchedFields.Contains(field))});
                        }

                        return indexSettings;
                    }).ToList();
                }

                return shapeHelper.EditorTemplate(TemplateName: "Parts/AdminSearch.SiteSettings", Model: model, Prefix: Prefix);
            }).OnGroup("admin search");
        }

        protected override void Exporting(AdminSearchSettingsPart part, ExportContentContext context) {
            context.Element(part.PartDefinition.Name).Add(new XAttribute("SearchedFields", string.Join(",", part.SearchedFields)));
        }

        protected override void Importing(AdminSearchSettingsPart part, ImportContentContext context) {
            var xElement = context.Data.Element(part.PartDefinition.Name);
            if (xElement == null) return;
            
            var searchedFields = xElement.Attribute("SearchedFields");
            if (searchedFields != null) {
                searchedFields.Remove();

                part.SearchedFields = searchedFields.Value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries); 
            }
        }
    }
}