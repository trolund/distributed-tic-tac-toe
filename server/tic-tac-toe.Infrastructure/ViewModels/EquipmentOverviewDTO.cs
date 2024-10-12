namespace Manufacturing.Infrastructure.ViewModels;

public class EquipmentOverviewDTO : EquipmentDTO
{
    public EquipmentStateDTO? State { get; set; }
    public DateTime ChangedAt { get; set; }
    public string ChangedBy { get; set; }
}