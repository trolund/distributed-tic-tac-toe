namespace Manufacturing.Infrastructure.ViewModels;

public class StateChangeHistoryDTO
{
    public int Id { get; set; }
    public int EquipmentId { get; set; }
    public EquipmentStateDTO State { get; set; }

    public DateTime ChangedAt { get; set; }
    public string ChangedBy { get; set; }

    public EquipmentDTO Equipment { get; set; }
}