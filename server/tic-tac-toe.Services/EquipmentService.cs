using AutoMapper;
using Manufacturing.Data.Models;
using Manufacturing.Data.Repositories.UnitOfWork;
using Manufacturing.Infrastructure.ViewModels;
using Manufacturing.Services.Interfaces;

namespace Manufacturing.Services;

public class EquipmentService(IUnitOfWork unitOfWork, IMapper mapper) : IEquipmentService
{
    public async Task<EquipmentDTO?> UpdateEquipments(EquipmentDTO model)
    {
        var dto = mapper.Map<Equipment>(model);
        await unitOfWork.Equipments.UpdateEquipments(dto);

        if (await unitOfWork.SaveAsync()) return model;

        return null;
    }

    public async Task<bool> UpdateEquipmentStatus(int equipmentId, EquipmentStateDTO newState)
    {
        var equipment = await unitOfWork.Equipments.GetEquipmentsById(equipmentId);

        if (equipment is null) return false;

        await unitOfWork.StateChangeHistories.Add(new StateChangeHistory
        {
            EquipmentId = equipmentId,
            State = mapper.Map<EquipmentState>(newState),
            ChangedAt = DateTime.Now,
            ChangedBy = "Admin" // use http context to get the current user
        });

        return await unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<EquipmentDTO>> GetAllEquipments()
    {
        return mapper.Map<IEnumerable<EquipmentDTO>>(await unitOfWork.Equipments.GetAllEquipments());
    }

    public async Task<EquipmentDTO> GetEquipmentsById(int customerId)
    {
        return mapper.Map<EquipmentDTO>(await unitOfWork.Equipments.GetEquipmentsById(customerId));
    }

    public async Task<List<EquipmentOverviewDTO>> GetEquipmentOverviews()
    {
        var equipments = await unitOfWork.Equipments.GetEquipmentsWithLatestState();
        return equipments.Select(e =>
        {
            var latestState = e.StateChangeHistories.FirstOrDefault();
            return new EquipmentOverviewDTO
            {
                Id = e.Id,
                State = latestState is null
                ? null
                : mapper.Map<EquipmentStateDTO>(latestState.State),
                Name = e.Name,
                Location = e.Location,
                ChangedAt = e.StateChangeHistories.FirstOrDefault()?.ChangedAt ?? DateTime.MinValue,
                ChangedBy = e.StateChangeHistories.FirstOrDefault()?.ChangedBy ?? "Unknown"
            };
        }).ToList();
    }

    public async Task<EquipmentOverviewDTO> GetEquipmentOverview(int equipmentId)
    {
        var equipment = await unitOfWork.Equipments.GetEquipmentsById(equipmentId);
        var stateHistories = await unitOfWork.StateChangeHistories.MostRecentEquipmentState(equipmentId);

        return new EquipmentOverviewDTO
        {
            Id = equipment.Id,
            State = stateHistories is null ? null : mapper.Map<EquipmentStateDTO>(stateHistories.State),
            Name = equipment.Name,
            Location = equipment.Location,
            ChangedAt = stateHistories?.ChangedAt ?? DateTime.MinValue,
            ChangedBy = stateHistories?.ChangedBy ?? "Unknown"
        };
    }
}