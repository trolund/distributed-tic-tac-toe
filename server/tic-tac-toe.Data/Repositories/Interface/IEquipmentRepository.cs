using Manufacturing.Data.Models;

namespace Manufacturing.Data.Repositories.Interface;

public interface IEquipmentRepository
{
    Task<IEnumerable<Equipment>> GetAllEquipments();
    Task<Equipment?> GetEquipmentsById(int equipmentId);
    Task<Equipment?> UpdateEquipments(Equipment model);
    Task<IEnumerable<Equipment>> GetEquipmentsWithLatestState();
}