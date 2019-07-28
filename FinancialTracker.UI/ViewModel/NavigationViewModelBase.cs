using FinancialTracker.UI.Event;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;

namespace FinancialTracker.UI.ViewModel
{
    public class NavigationViewModelBase
    {
        protected IEventAggregator _eventAggregator;

        public NavigationViewModelBase(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        protected void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items, AfterDetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(x => x.ID == args.ID);
            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(args.ID, args.DisplayMember, args.ViewModelName, _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }

        protected void AfterDetailDeleted(ObservableCollection<NavigationItemViewModel> items, AfterDetailDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(x => x.ID == args.ID);
            if (item != null)
            {
                items.Remove(item);
            }
        }
    }
}
