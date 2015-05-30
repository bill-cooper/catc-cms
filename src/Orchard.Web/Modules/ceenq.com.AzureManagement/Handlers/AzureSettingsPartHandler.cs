using ceenq.com.AzureManagement.Models;
using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace ceenq.com.AzureManagement.Handlers {
    [UsedImplicitly]
    public class AzureSettingsPartHandler : ContentHandler {
        public AzureSettingsPartHandler(IRepository<AzureSettingsRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<AzureSettingsPart>("Account"));
        }
    }
}