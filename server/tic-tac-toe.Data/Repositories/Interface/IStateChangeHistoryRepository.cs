using Manufacturing.Data.Models;

namespace Manufacturing.Data.Repositories.Interface;

public interface IStateChangeHistoryRepository : IRepository<StateChangeHistory, Guid>
{
    Task<StateChangeHistory?> MostRecentEquipmentState(int equipmentId);
    Task<List<StateChangeHistory>> GetMostResentStateChangeHistories(int numberOfHistories);
}