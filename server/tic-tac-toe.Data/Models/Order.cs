namespace Manufacturing.Data.Models;

public class Order
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime ScheduledStartTime { get; set; }
    public DateTime ScheduledEndTime { get; set; }

    // Foreign key for related equipment
    public int EquipmentId { get; set; }

    // Navigation property for related equipment
    public Equipment Equipment { get; set; }
}