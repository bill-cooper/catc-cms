using System;
using System.ComponentModel.DataAnnotations;

namespace ceenq.org.Resource.ViewModels
{
    public class ResourcePartViewModel
    {
        [Required]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? DeliveredUtc { get; set; }
        public string CorrespondingTexts { get; set; }

    }
}