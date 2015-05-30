using System.ComponentModel.DataAnnotations;

namespace ceenq.com.RoutingServer.ViewModels
{
    public class ConfigEditViewModel
    {
        [Required, StringLength(50)]
        public string FullName { get; set; }
        public string Text { get; set; }
    }
}