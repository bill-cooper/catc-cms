using System.Collections.Generic;

namespace ceenq.com.AssetImport.ViewModels
{
    public class ImportViewModel {
        public string Subdirectory { get; set; }

        public bool OverwriteExisting { get; set; }

        public string ImportUrl { get; set; }
    }
}
