using AutoMapper;
using ProvaPortal.Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ProfessorModel, ProfessorDTO>().ReverseMap();
    }
}
