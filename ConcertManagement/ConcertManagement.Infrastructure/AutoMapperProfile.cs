﻿using AutoMapper;
using ConcertManagement.Core.Dtos;
using ConcertManagement.Core.Entities;

namespace ConcertManagement.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // VenueDto -> Venue
            CreateMap<VenueDto, Venue>()
            .AfterMap((s, d) =>
            {
                if (d.CreatedDate == DateTime.MinValue) d.CreatedDate = DateTime.UtcNow;
                if (d.UpdatedDate == DateTime.MinValue) d.UpdatedDate = DateTime.UtcNow;
                if (string.IsNullOrWhiteSpace(d.CreatedBy)) d.CreatedBy = "admin"; // assumption for now
                if (string.IsNullOrWhiteSpace(d.UpdatedBy)) d.UpdatedBy = "admin"; // assumption for now
            });

            // EventDto -> Event
            CreateMap<EventDto, Event>()
            .ForMember(dest => dest.Venue, opt => opt.Ignore())
            .ForMember(dest => dest.Reservations, opt => opt.Ignore())
            .ForMember(dest => dest.TicketTypes, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                if (d.CreatedDate == DateTime.MinValue) d.CreatedDate = DateTime.UtcNow;
                if (d.UpdatedDate == DateTime.MinValue) d.UpdatedDate = DateTime.UtcNow;
                if (string.IsNullOrWhiteSpace(d.CreatedBy)) d.CreatedBy = "admin"; // assumption for now
                if (string.IsNullOrWhiteSpace(d.UpdatedBy)) d.UpdatedBy = "admin"; // assumption for now
            });

            // Event -> EventDto
            CreateMap<Event, EventDto>()
             .ForMember(dest => dest.VenueName, opt => opt.MapFrom(src => src.Venue.Name))
             .ForMember(dest => dest.TicketTypes, opt => opt.MapFrom(src => src.TicketTypes));

            // TicketTypeDto -> TicketType
            CreateMap<TicketTypeDto, TicketType>()
            .ForMember(dest => dest.Reservations, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                if (d.CreatedDate == DateTime.MinValue) d.CreatedDate = DateTime.UtcNow;
                if (d.UpdatedDate == DateTime.MinValue) d.UpdatedDate = DateTime.UtcNow;
                if (string.IsNullOrWhiteSpace(d.CreatedBy)) d.CreatedBy = "admin"; // assumption for now
                if (string.IsNullOrWhiteSpace(d.UpdatedBy)) d.UpdatedBy = "admin"; // assumption for now
            });

            // ReservationRequest -> Reservation
            CreateMap<ReservationRequest, Reservation>()
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                if (d.CreatedDate == DateTime.MinValue) d.CreatedDate = DateTime.UtcNow;
                if (d.UpdatedDate == DateTime.MinValue) d.UpdatedDate = DateTime.UtcNow;
                if (string.IsNullOrWhiteSpace(d.CreatedBy)) d.CreatedBy = "admin"; // assumption for now
                if (string.IsNullOrWhiteSpace(d.UpdatedBy)) d.UpdatedBy = "admin"; // assumption for now
            });

            // Reservation -> ReservationDto
            CreateMap<Reservation, ReservationDto>()
             .ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src.Tickets))
             .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.Payments));

            // TicketDto -> Ticket
            CreateMap<TicketDto, Ticket>()
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                if (d.CreatedDate == DateTime.MinValue) d.CreatedDate = DateTime.UtcNow;
                if (d.UpdatedDate == DateTime.MinValue) d.UpdatedDate = DateTime.UtcNow;
                if (string.IsNullOrWhiteSpace(d.CreatedBy)) d.CreatedBy = "admin"; // assumption for now
                if (string.IsNullOrWhiteSpace(d.UpdatedBy)) d.UpdatedBy = "admin"; // assumption for now
            });

            // PaymentDto -> Payment
            CreateMap<PaymentDto, Payment>()
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                if (d.CreatedDate == DateTime.MinValue) d.CreatedDate = DateTime.UtcNow;
                if (d.UpdatedDate == DateTime.MinValue) d.UpdatedDate = DateTime.UtcNow;
                if (string.IsNullOrWhiteSpace(d.CreatedBy)) d.CreatedBy = "admin"; // assumption for now
                if (string.IsNullOrWhiteSpace(d.UpdatedBy)) d.UpdatedBy = "admin"; // assumption for now
            });
        }
    }
}
