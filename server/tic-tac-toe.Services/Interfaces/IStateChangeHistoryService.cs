using Manufacturing.Infrastructure.ViewModels;

namespace Manufacturing.Services.Interfaces;

public interface IStateChangeHistoryService
{
    Task<IEnumerable<StateChangeHistoryDTO>> GetAllStateChangeHistory();
    Task<IEnumerable<StateChangeHistoryDTO>> GetMostResentStateChangeHistory(int numberOfHistories);
}