using Practic.Models;

namespace Practic.ViewModels
{
    public class CurrencyViewModel
    {
        public IEnumerable<Currency> currencies {  get; set; }
        public PageViewModel PageViewModel { get; set; }

        public string NameofCurrency { get; set; }

        public string CountryofCurrency { get; set; } 

    }
}
