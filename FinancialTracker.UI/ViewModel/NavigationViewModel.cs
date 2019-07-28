using System.Threading.Tasks;

namespace FinancialTracker.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        public IPaymentNavigationViewModel PaymentNavigationViewModel { get; }

        public IRecipientNavigationViewModel RecipientNavigationViewModel { get; }

        public IPaymentPurposeNavigationViewModel PaymentPurposeNavigationViewModel { get; } 

        public NavigationViewModel(IPaymentNavigationViewModel paymentNavigationViewModel,
            IRecipientNavigationViewModel recipientNavigationViewModel,
            IPaymentPurposeNavigationViewModel paymentPurposeNavigationViewModel)
        {
            PaymentNavigationViewModel = paymentNavigationViewModel;
            RecipientNavigationViewModel = recipientNavigationViewModel;
            PaymentPurposeNavigationViewModel = paymentPurposeNavigationViewModel;
        }

        public async Task LoadAsync()
        {
            await PaymentNavigationViewModel.LoadAsync();
            await RecipientNavigationViewModel.LoadAsync();
            await PaymentPurposeNavigationViewModel.LoadAsync();
        }
    }
}
