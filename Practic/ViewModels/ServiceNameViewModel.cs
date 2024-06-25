using Practic.Models;

namespace Practic.ViewModels
{
    public class ServiceNameViewModel
    {
        public IEnumerable<ServiceName> serviceNames {  get; set; }
        public PageViewModel PageViewModel { get; set; }

        public string NameofService { get; set; } 

        public string Department { get; set; } 
    }
}
