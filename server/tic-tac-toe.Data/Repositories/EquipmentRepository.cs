using Manufacturing.Data.Contexts;
using Manufacturing.Data.Models;
using Manufacturing.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Manufacturing.Data.Repositories;

public class EquipmentRepository(ApplicationDbContext context)
    : Repository<Equipment, Guid>(context), IEquipmentRepository
{
    public Task<Equipment?> UpdateEquipments(Equipment model)
    {
        var result = context.Equipments.Update(model);
        return Task.FromResult(result.State != EntityState.Modified ? null : result.Entity);
    }

    public async Task<IEnumerable<Equipment>> GetAllEquipments()
    {
        return await context.Equipments.ToListAsync();
    }

    public async Task<Equipment?> GetEquipmentsById(int equipmentId)
    {
        return await context.Equipments.FindAsync(equipmentId);
    }
    
    public async Task<IEnumerable<Equipment>> GetEquipmentsWithLatestState()
    {   
        return await context.Equipments
            .Include(e => e.StateChangeHistories
                .OrderByDescending(x => x.ChangedAt)
                .Take(1))
            .ToListAsync();
    }
}