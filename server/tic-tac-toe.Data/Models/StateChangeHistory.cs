namespace Manufacturing.Data.Models;

public class StateChangeHistory
{
    public int Id { get; set; }
    public int EquipmentId { get; set; }
    public EquipmentState State { get; set; }

    public DateTime ChangedAt { get; set; }
    public string ChangedBy { get; set; }

    // Navigation property for related equipment
    public Equipment Equipment { get; set; }
}