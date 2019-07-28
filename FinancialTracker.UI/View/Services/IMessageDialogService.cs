using System.Threading.Tasks;

namespace FinancialTracker.UI.View.Services
{
    public interface IMessageDialogService
    {
        Task<MessageDialogResult> ShowOkCancelDialogAsync(string text, string title);

        Task ShowInfoDialogAsync(string text);
    }
}