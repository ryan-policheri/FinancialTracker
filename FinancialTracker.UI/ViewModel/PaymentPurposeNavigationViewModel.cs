using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FinancialTracker.Model;
using FinancialTracker.UI.Data.Lookups;
using FinancialTracker.UI.Event;
using Prism.Events;

namespace FinancialTracker.UI.ViewModel
{
    public class PaymentPurposeNavigationViewModel : NavigationViewModelBase, IPaymentPurposeNavigationViewModel
    {
        private IPaymentPurposeLookupDataService _paymentPurposeLookupDataService;

        public ObservableCollection<NavigationItemViewModel> PaymentPurposes { get; }

        public PaymentPurposeNavigationViewModel(IPaymentPurposeLookupDataService paymentPurposeLookupDataService, IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _paymentPurposeLookupDataService = paymentPurposeLookupDataService;

            PaymentPurposes = new ObservableCollection<NavigationItemViewModel>();

            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        public async Task LoadAsync()
        {
            IEnumerable<LookupItem> lookup = await _paymentPurposeLookupDataService.GetPaymentPurposeLookupAsync();
            PaymentPurposes.Clear();
            foreach (LookupItem lookupItem in lookup)
            {
                var navigationItemViewModel = new NavigationItemViewModel(lookupItem.ID, lookupItem.DisplayMember, nameof(PaymentPurposeDetailViewModel), _eventAggregator);
                PaymentPurposes.Add(navigationItemViewModel);
            }
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(PaymentPurposeDetailViewModel))
            {
                AfterDetailSaved(PaymentPurposes, args);
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(PaymentPurposeDetailViewModel))
            {
                AfterDetailDeleted(PaymentPurposes, args);
            }
        }
    }
}
