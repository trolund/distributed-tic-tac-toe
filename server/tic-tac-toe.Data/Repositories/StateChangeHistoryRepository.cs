using Manufacturing.Data.Contexts;
using Manufacturing.Data.Models;
using Manufacturing.Data.Repositories.Interface;

namespace Manufacturing.Data.Repositories;

public class StateChangeHistoryRepository(ApplicationDbContext context)
    : Repository<StateChangeHistory, Guid>(context), IStateChangeHistoryRepository
{
    public Task<StateChangeHistory?> MostRecentEquipmentState(int equipmentId)
    {
        return Task.FromResult(context.StateChangeHistories
            .Where(x => x.EquipmentId == equipmentId)
            .OrderByDescending(x => x.ChangedAt)
            .FirstOrDefault());
    }
    
    public Task<List<StateChangeHistory>> GetMostResentStateChangeHistories(int numberOfHistories)
    {
        return Task.FromResult(context.StateChangeHistories
            .OrderByDescending(x => x.ChangedAt)
            .Take(numberOfHistories)
            .ToList());
    }
}