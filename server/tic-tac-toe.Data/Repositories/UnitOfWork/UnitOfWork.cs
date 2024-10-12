using Manufacturing.Data.Contexts;
using Manufacturing.Data.Repositories.Interface;
using Microsoft.Extensions.Logging;

namespace Manufacturing.Data.Repositories.UnitOfWork;

public class UnitOfWork(
    ApplicationDbContext context,
    ILogger<UnitOfWork> logger,
    IStateChangeHistoryRepository stateChangeHistoryRepository,
    // ITimeRegistrationRepository timeRegistrationRepository,
    // IUserRepository userRepository,
    IEquipmentRepository equipmentRepository)
    : IUnitOfWork
{
    // repos
    public IStateChangeHistoryRepository StateChangeHistories { get; } = stateChangeHistoryRepository;
    // public ITimeRegistrationRepository TimeRegistrations { get; } = timeRegistrationRepository;
    // public IUserRepository Users { get; } = userRepository;

    public IEquipmentRepository Equipments { get; } = equipmentRepository;

    public bool Save()
    {
        return context.SaveChanges() >= 0;
    }

    public async Task<bool> SaveAsync()
    {
        return await context.SaveChangesAsync() >= 0;
    }

    public void Dispose()
    {
        try
        {
            context.Dispose();
        }
        catch (Exception e)
        {
            logger.LogError("Dispose UnitOfWork", e, "faild to dispose unit of work");
        }
    }
}