using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ceenq.com.Assets.Models
{
    public class AssetModel
    {
        public virtual string Id { get; set; }
        public virtual string Path { get; set; }

        public virtual string Body { get; set; }

        public virtual string ContentType { get; set; }
    }
}