using Prism.Events;

namespace FinancialTracker.UI.Event
{
    public class AfterDetailClosedEvent : PubSubEvent<AfterDetailClosedEventArgs>
    {
    }

    public class AfterDetailClosedEventArgs
    {
        public int ID { get; set; }

        public string ViewModelName { get; set; }
    }
}
