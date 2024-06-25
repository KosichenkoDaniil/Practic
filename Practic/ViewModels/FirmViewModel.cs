using Practic.Models;

namespace Practic.ViewModels
{
    public class FirmViewModel
    {
        public IEnumerable<Firm> firms { get; set; }
        public PageViewModel PageViewModel { get; set; }

        public string СountryofFirm { get; set; } 

        public string NameofFirm { get; set; } 
    }
}
