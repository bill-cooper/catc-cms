using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ceenq.com.Assets.Models
{
    public class DirectoryItemModel
    {
        public virtual string Id { get; set; }
        public string Name { get; set; }
        public virtual string Path { get; set; }
        public virtual string Type { get; set; }
        public virtual string ContentType { get; set; }
        public IEnumerable<DirectoryItemModel> Children { get; set; }

        //public DirectoryItemModel Parent { get; set; }
    }
}