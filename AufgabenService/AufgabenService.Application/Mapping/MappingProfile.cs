using AutoMapper;
using AufgabenService.Application.DTOs;
using AufgabenService.Domain.Entities;

namespace AufgabenService.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain -> DTO
            CreateMap<Aufgabe, AufgabeDto>();
            CreateMap<Antwort, AntwortDto>();

            // DTO -> Domain
            CreateMap<AufgabeErstellenDto, Aufgabe>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Antworten, opt => opt.Ignore());

            CreateMap<AufgabeAktualisierenDto, Aufgabe>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Antworten, opt => opt.Ignore());
            
            CreateMap<AntwortErstellenDto, Antwort>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}