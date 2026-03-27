using AutoMapper;
using ResourceHub.Core.Entities;
using ResourceHub.Api.DTOs;

namespace ResourceHub.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Booking -> BookingDto
            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.ResourceName,
                            opt => opt.MapFrom(src => src.Resource.Name));

            // CreateBookingDto -> Booking
            CreateMap<CreateBookingDto, Booking>()
                .ConstructUsing(dto => new Booking(
                    dto.ResourceId,
                    dto.StartTime,
                    dto.EndTime,
                    dto.BookedBy,
                    dto.Purpose
                ));


            // Resource -> ResourceDto
            CreateMap<Resource, ResourceDto>();

            // CreateResourceDto -> Resource
            CreateMap<CreateResourceDto, Resource>()
                .ConstructUsing(dto => new Resource(
                    dto.Name,
                    dto.Description,
                    dto.Location,
                    dto.Capacity
                ));
        }
    }
}