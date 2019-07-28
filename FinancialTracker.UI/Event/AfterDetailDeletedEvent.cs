using Prism.Events;

namespace FinancialTracker.UI.Event
{
    public class AfterDetailDeletedEvent : PubSubEvent<AfterDetailDeletedEventArgs>
    {
    }

    public class AfterDetailDeletedEventArgs
    {
        public int ID { get; set; }
        public string ViewModelName { get; set; }
    }
}
