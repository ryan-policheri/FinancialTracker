using System.Windows.Input;
using FinancialTracker.UI.Event;
using Prism.Commands;
using Prism.Events;

namespace FinancialTracker.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private string _displayMember;
        private string _detailViewModelName;
        private IEventAggregator _eventAggregator;

        public int ID { get; }

        public string DisplayMember
        {
            get { return _displayMember; }
            set
            {
                _displayMember = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenDetailViewCommand { get; }

        public NavigationItemViewModel(int id, string displayMember, string detailViewModelName, IEventAggregator eventAggregator)
        {
            _detailViewModelName = detailViewModelName;
            _eventAggregator = eventAggregator;

            ID = id;
            DisplayMember = displayMember;
            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
        }

        private void OnOpenDetailViewExecute()
        {
            _eventAggregator.GetEvent<OpenDetailViewEvent>()
                .Publish(new OpenDetailViewEventArgs
                {
                    ID = ID,
                    ViewModelName = _detailViewModelName
                });
        }
    }
}
