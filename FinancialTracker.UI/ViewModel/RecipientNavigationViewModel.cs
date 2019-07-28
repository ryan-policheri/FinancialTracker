using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FinancialTracker.Model;
using FinancialTracker.UI.Data.Lookups;
using FinancialTracker.UI.Event;
using Prism.Events;

namespace FinancialTracker.UI.ViewModel
{
    public class RecipientNavigationViewModel : NavigationViewModelBase, IRecipientNavigationViewModel
    {
        private IRecipientLookupDataService _recipientLookupService;

        public ObservableCollection<NavigationItemViewModel> Recipients { get; }

        public RecipientNavigationViewModel(IRecipientLookupDataService recipientLookupService, IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _recipientLookupService = recipientLookupService;

            Recipients = new ObservableCollection<NavigationItemViewModel>();

            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        public async Task LoadAsync()
        {
            IEnumerable<LookupItem> lookup = await _recipientLookupService.GetRecipientLookupAsync();
            Recipients.Clear();
            foreach (LookupItem lookupItem in lookup)
            {
                var navigationItemViewModel = new NavigationItemViewModel(lookupItem.ID, lookupItem.DisplayMember, nameof(RecipientDetailViewModel), _eventAggregator);
                Recipients.Add(navigationItemViewModel);
            }
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(RecipientDetailViewModel))
            {
                AfterDetailSaved(Recipients, args);
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(RecipientDetailViewModel))
            {
                AfterDetailDeleted(Recipients, args);
            }
        }
    }
}
