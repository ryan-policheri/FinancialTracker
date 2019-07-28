using System.Threading.Tasks;

namespace FinancialTracker.UI.Data.Repositories
{
    public interface IGenericRepository<T>
    {
        Task<T> GetByIDAsync(int id);
        Task SaveAsync();
        bool HasChanges();
        void Add(T model);
        void Remove(T model);
    }
}