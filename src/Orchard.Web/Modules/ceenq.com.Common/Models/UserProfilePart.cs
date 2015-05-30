using Orchard.ContentManagement;

namespace ceenq.com.Common.Models
{
    public sealed class UserProfilePart : ContentPart<UserProfilePartRecord>
    {
        public string Name
        {
            get { return Record.Name; }
            set { Record.Name = value; }
        }
    }
}