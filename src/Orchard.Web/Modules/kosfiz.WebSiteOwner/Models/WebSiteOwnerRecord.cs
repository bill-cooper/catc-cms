using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;
using System.ComponentModel.DataAnnotations;

namespace kosfiz.WebSiteOwner.Models
{
    public class WebSiteOwnerRecord
    {
        public virtual int Id { get; set; }
        [Required]
        public virtual string Title { get; set; }
        [Required]
        public virtual string MetaName { get; set; }
        [Required]
        public virtual string MetaContent { get; set; }
    }
}