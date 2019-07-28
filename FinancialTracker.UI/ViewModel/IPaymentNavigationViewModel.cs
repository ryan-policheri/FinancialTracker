using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FinancialTracker.UI.ViewModel
{
    public interface IPaymentNavigationViewModel : INavigationViewModel
    {
        ObservableCollection<NavigationItemViewModel> Payments { get; }
    }
}