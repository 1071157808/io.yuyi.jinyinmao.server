// FileInformation: nyanya/Domain/IReadModelDataService.cs
// CreatedTime: 2014/06/09   6:32 PM
// LastUpdatedTime: 2014/06/10   11:33 AM

using System.Threading.Tasks;

namespace Domain.ReadModel.Interface
{
    public interface IReadModelDataService : IDataService
    {
    }

    public interface IReadModelDataService<T, in T1> : IReadModelDataService<T> where T : IReadModel, IQueryableFromWarehouse<T1>
    {
        Task<T> GetReadModel(T1 t);
    }

    public interface IReadModelDataService<T> : IReadModelDataService where T : IReadModel
    {
    }
}