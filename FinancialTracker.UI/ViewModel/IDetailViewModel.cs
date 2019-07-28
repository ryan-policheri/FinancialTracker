using System.Threading.Tasks;

namespace FinancialTracker.UI.ViewModel
{
    public interface IDetailViewModel
    {
        bool HasChanges { get; }
        int ID { get; }

        Task LoadAsync(int ID);
    }
}