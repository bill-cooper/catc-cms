using System;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace ceenq.org.Services.Models
{
    public sealed class ESVBibleServiceSettingsPart : ContentPart<ESVBibleServiceSettingsPartRecord>
    {
        public string EsvBibleServiceKey
        {
            get { return Record.EsvBibleServiceKey; }
            set { Record.EsvBibleServiceKey = value; }
        }
    }

    public class ESVBibleServiceSettingsPartRecord : ContentPartRecord
    {
        public virtual string EsvBibleServiceKey { get; set; }
    }
}