using FinancialTracker.UI.Data.Lookups;
using System.Collections.Generic;
using System.Threading.Tasks;
using Prism.Events;
using System.Collections.ObjectModel;
using FinancialTracker.UI.Event;
using FinancialTracker.Model;

namespace FinancialTracker.UI.ViewModel
{
    public class PaymentNavigationViewModel : NavigationViewModelBase, IPaymentNavigationViewModel
    {
        private IPaymentLookupDataService _paymentLookupService;

        public ObservableCollection<NavigationItemViewModel> Payments { get; }

        public PaymentNavigationViewModel(IPaymentLookupDataService paymentLookupService, IEventAggregator eventAggregator) 
            : base(eventAggregator)
        {
            _paymentLookupService = paymentLookupService;

            Payments = new ObservableCollection<NavigationItemViewModel>();

            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        public async Task LoadAsync()
        {
            IEnumerable<LookupItem> lookup = await _paymentLookupService.GetPaymentLookupAsync();
            Payments.Clear();
            foreach (LookupItem lookupItem in lookup)
            {
                var navigationItemViewModel = new NavigationItemViewModel(lookupItem.ID, lookupItem.DisplayMember, nameof(PaymentDetailViewModel), _eventAggregator);
                Payments.Add(navigationItemViewModel);
            }
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(PaymentDetailViewModel))
            {
                AfterDetailSaved(Payments, args);
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(PaymentDetailViewModel))
            {
                AfterDetailDeleted(Payments, args);
            }
        }
    }
}
