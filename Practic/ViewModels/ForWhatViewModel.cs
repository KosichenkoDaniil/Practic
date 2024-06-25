using Practic.Models;

namespace Practic.ViewModels
{
    public class ForWhatViewModel
    {
        public IEnumerable<ForWhat> forWhats { get; set; }
        public PageViewModel PageViewModel { get; set; }

        public string TypeofWork { get; set; } 
    }
}
