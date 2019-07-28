namespace FinancialTracker.Model
{
    public class LookupItem
    {
        public int ID { get; set; }

        public string DisplayMember { get; set; }
    }

    public class NullLookupItem : LookupItem
    {
        public new int? ID
        {
            get { return null; }
        }
    }
}
