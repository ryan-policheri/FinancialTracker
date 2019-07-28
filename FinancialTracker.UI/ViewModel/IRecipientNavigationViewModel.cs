using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FinancialTracker.UI.ViewModel
{
    public interface IRecipientNavigationViewModel : INavigationViewModel
    {
        ObservableCollection<NavigationItemViewModel> Recipients { get; }
    }
}