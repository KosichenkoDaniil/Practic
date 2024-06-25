using Practic.Models;

namespace Practic.ViewModels
{
    public class ApplicationViewModel
    {
        public IEnumerable<Application> applications { get; set; }
        public PageViewModel PageViewModel { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string NameofFirm { get; set; }

        public string NameofCurrency { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
