using Practic.Models;

namespace Practic.ViewModels
{
    public class FabricViewModel
    {
        public IEnumerable<Fabric> fabrics { get; set; }
        public PageViewModel PageViewModel { get; set; }

        public string Name { get; set; }

        public string NameofService { get; set; }

        public string TypeofWork { get; set; }

        public string CodeTnved { get; set; } 

        public string CodeOkrb { get; set; } 

    }
}
