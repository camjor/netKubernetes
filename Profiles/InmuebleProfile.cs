using AutoMapper;
using NetCamjor.Dtos.InmuebleDtos;
using NetCamjor.Models;

namespace NetCamjor.Profiles;


public class InmuebleProfile : Profile {
    public InmuebleProfile()
    {
        CreateMap<Inmueble,InmuebleResponseDto>();
        CreateMap<InmuebleRequestDto,Inmueble>();
    }
}