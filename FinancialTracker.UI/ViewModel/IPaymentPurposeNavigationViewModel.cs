using System.Collections.ObjectModel;

namespace FinancialTracker.UI.ViewModel
{
    public interface IPaymentPurposeNavigationViewModel : INavigationViewModel
    {
        ObservableCollection<NavigationItemViewModel> PaymentPurposes { get; }
    }
}