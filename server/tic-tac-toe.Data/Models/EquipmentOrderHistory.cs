namespace Manufacturing.Data.Models;

public class EquipmentOrderHistory
{
    public int Id { get; set; }
    public int EquipmentId { get; set; }
    public int OrderId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }

    // Navigation properties
    public Equipment Equipment { get; set; }
    public Order Order { get; set; }
}