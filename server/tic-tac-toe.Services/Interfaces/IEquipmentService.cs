using Manufacturing.Infrastructure.ViewModels;

namespace Manufacturing.Services.Interfaces;

public interface IEquipmentService
{
    Task<IEnumerable<EquipmentDTO>> GetAllEquipments();
    Task<EquipmentDTO> GetEquipmentsById(int equipmentsId);
    Task<EquipmentDTO?> UpdateEquipments(EquipmentDTO model);
    Task<bool> UpdateEquipmentStatus(int equipmentId, EquipmentStateDTO newState);
    Task<List<EquipmentOverviewDTO>> GetEquipmentOverviews();
    Task<EquipmentOverviewDTO> GetEquipmentOverview(int equipmentId);
}