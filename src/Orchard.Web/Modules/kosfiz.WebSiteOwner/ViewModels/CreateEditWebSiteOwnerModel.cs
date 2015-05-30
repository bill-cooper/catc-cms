using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace kosfiz.WebSiteOwner.ViewModels
{
    public class CreateEditWebSiteOwnerModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string MetaName { get; set; }
        [Required]
        public string MetaContent { get; set; }
    }
}