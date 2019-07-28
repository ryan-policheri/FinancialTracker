using Prism.Events;

namespace FinancialTracker.UI.Event
{
    public class AfterDetailSavedEvent : PubSubEvent<AfterDetailSavedEventArgs>
    {
    }

    public class AfterDetailSavedEventArgs
    {
        public int ID { get; set; }

        public string DisplayMember { get; set; }

        public string ViewModelName { get; set; }
    }
}
