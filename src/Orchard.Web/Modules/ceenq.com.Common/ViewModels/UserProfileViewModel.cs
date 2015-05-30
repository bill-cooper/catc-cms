using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;
using ceenq.com.Common.Models;

namespace ceenq.com.Common.ViewModels
{
    public class UserProfileViewModel
    {
        [Required]
        public string Name {get; set; }

    }
}