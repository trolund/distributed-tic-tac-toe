using AutoMapper;
using Manufacturing.Data.Models;
using Manufacturing.Infrastructure.ViewModels;

namespace Manufacturing.Data.Mappings;

public class Profiles : Profile
{
    public Profiles()
    {
        CreateMap<Equipment, EquipmentDTO>().ReverseMap();
        CreateMap<StateChangeHistory, StateChangeHistoryDTO>().ReverseMap();
        CreateMap<EquipmentState, EquipmentStateDTO>().ReverseMap();
    }
}