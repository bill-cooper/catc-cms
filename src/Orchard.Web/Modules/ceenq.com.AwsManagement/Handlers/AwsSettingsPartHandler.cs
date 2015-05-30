using ceenq.com.AwsManagement.Models;
using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace ceenq.com.AwsManagement.Handlers {
    [UsedImplicitly]
    public class AwsSettingsPartHandler : ContentHandler {
        public AwsSettingsPartHandler(IRepository<AwsSettingsRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<AwsSettingsPart>("Account"));
        }
    }
}