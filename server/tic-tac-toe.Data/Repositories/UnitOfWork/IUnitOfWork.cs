using Manufacturing.Data.Repositories.Interface;

namespace Manufacturing.Data.Repositories.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IStateChangeHistoryRepository StateChangeHistories { get; }
    IEquipmentRepository Equipments { get; }

    bool Save();
    Task<bool> SaveAsync();
}