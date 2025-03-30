using AutoMapper;
using hotel_booking_dto.AmenityDtos;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_dto.BedTypeDtos;
using hotel_booking_dto.BookingDtos;
using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.ImageDtos;
using hotel_booking_dto.ManagerDtos;
using hotel_booking_dto.ManagerRequestDtos;
using hotel_booking_dto.ReviewDtos;
using hotel_booking_dto.RoomDtos;
using hotel_booking_dto.RoomTypeAmenityDtos;
using hotel_booking_dto.RoomTypeDtos;
using hotel_booking_models;


namespace thda.Utilities.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Authentication Maps
            CreateMap<AppUser, RegisterDto>().ReverseMap();
            CreateMap<AppUser, LoginDto>().ReverseMap();
            CreateMap<AppUser, ResetPasswordDto>().ReverseMap();
            CreateMap<AppUser, UpdatePasswordDto>().ReverseMap();

            //Amenity Maps
            CreateMap<Amenity, AmenityDto>()
                .ReverseMap();
            CreateMap<Amenity, AmenityCreateDto>().ReverseMap();
            CreateMap<Amenity, AmenityUpdateDto>().ReverseMap();
            CreateMap<Amenity, AmenityResponseDto>().ReverseMap();

            //Review Maps
            CreateMap<Review, AddReviewDto>().ReverseMap();
            CreateMap<Review, ReviewUpdateDto>().ReverseMap();

            CreateMap<Review, ReviewDto>()
                .ForMember(x => x.FirstName, x => x.MapFrom(x => x.Customer.AppUser.FirstName))
                .ForMember(x => x.LastName, x => x.MapFrom(x => x.Customer.AppUser.LastName))
                .ForMember(x => x.Avatar, x => x.MapFrom(x => x.Customer.AppUser.Avatar));

            //Hotel Maps
            CreateMap<Hotel, UpdateHotelDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();
            CreateMap<Hotel, HotelResponseDto>().ReverseMap();
            CreateMap<Hotel, GetAllHotelDto>()
                .ForMember(x => x.ReviewRating, y => y.MapFrom(z => z.Reviews.Any() ? z.Reviews.Average(r => r.Rating) : 0))
                .ForMember(x => x.ReviewCount, y => y.MapFrom(z => z.Reviews.Count()))
                .ForMember(x => x.Price, y => y.MapFrom(z =>
                    z.RoomTypes.SelectMany(rt => rt.Rooms).Any()
                        ? z.RoomTypes.SelectMany(rt => rt.Rooms)
                            .OrderBy(r => r.Discount ?? r.Price)
                            .ThenBy(r => r.Price)
                            .Select(r => r.Price)
                            .First()
                        : 0
                ))
                .ForMember(x => x.Discount, y => y.MapFrom(z =>
                    z.RoomTypes.SelectMany(rt => rt.Rooms).Any()
                        ? z.RoomTypes.SelectMany(rt => rt.Rooms)
                            .OrderBy(r => r.Discount ?? r.Price)
                            .ThenBy(r => r.Price)
                            .Select(r => r.Discount)
                            .First()
                        : null
                ))
                .ForMember(x => x.Location, y => y.MapFrom(z =>
                    z.Ward != null ? z.Ward.Name + " Ward" :
                    z.District != null ? z.District.Name + " District" :
                    z.Province != null ? z.Province.Name + " Province" : ""
                ));
            CreateMap<Hotel, HotelDetailResponseDto>()
               .ForMember(x => x.Price, y => y.MapFrom(z =>
                   z.RoomTypes.SelectMany(rt => rt.Rooms).Any()
                       ? z.RoomTypes.SelectMany(rt => rt.Rooms)
                           .OrderBy(r => r.Discount ?? r.Price)
                           .ThenBy(r => r.Price)
                           .Select(r => r.Price)
                           .First()
                       : 0
               ))
               .ForMember(x => x.Discount, y => y.MapFrom(z =>
                   z.RoomTypes.SelectMany(rt => rt.Rooms).Any()
                       ? z.RoomTypes.SelectMany(rt => rt.Rooms)
                           .OrderBy(r => r.Discount ?? r.Price)
                           .ThenBy(r => r.Price)
                           .Select(r => r.Discount)
                           .First()
                       : null
               ))
               .ForMember(x => x.Province, y => y.MapFrom(z =>
                   z.Province.Name
               ))
               .ForMember(x => x.District, y => y.MapFrom(z =>
                   z.District.Name
               ))
               .ForMember(x => x.Ward, y => y.MapFrom(z =>
                   z.Ward.Name
               ));

            //Manager Request Maps
            CreateMap<ManagerRequest, AddManagerRequestDto>().ReverseMap();
            CreateMap<ManagerRequest, ManagerRequestResponseDto>();

            //Manager Maps
            CreateMap<Manager, RegisterManagerDto>().ReverseMap();
            CreateMap<AppUser, RegisterManagerDto>()
                .ForMember(manager => manager.BusinessEmail, u => u.MapFrom(user => user.Email))
                .ForMember(manager => manager.BusinessEmail, u => u.MapFrom(user => user.UserName))
                .ForMember(manager => manager.BusinessPhone, u => u.MapFrom(user => user.PhoneNumber))
                .ReverseMap();
            CreateMap<Hotel, RegisterManagerDto>()
                .ForMember(hotel => hotel.HotelAddress, u => u.MapFrom(hotel => hotel.Address))
                .ForMember(hotel => hotel.HotelCity, u => u.MapFrom(hotel => hotel.City))
                .ForMember(hotel => hotel.HotelDescription, u => u.MapFrom(hotel => hotel.Description))
                .ForMember(hotel => hotel.HotelEmail, u => u.MapFrom(hotel => hotel.Email))
                .ForMember(hotel => hotel.HotelName, u => u.MapFrom(hotel => hotel.Name))
                .ForMember(hotel => hotel.HotelPhone, u => u.MapFrom(hotel => hotel.Phone))
                .ReverseMap();
            CreateMap<Manager, ManagerDetailsResponseDto>()
                .ForMember(x => x.FirstName, o => o.MapFrom(y => y.AppUser.FirstName))
                .ForMember(x => x.LastName, o => o.MapFrom(y => y.AppUser.LastName))
                .ForMember(x => x.ManagerPhone, o => o.MapFrom(y => y.AppUser.PhoneNumber))
                .ForMember(x => x.CompanyName, o => o.MapFrom(y => y.CompanyName))
                .ForMember(x => x.BusinessEmail, o => o.MapFrom(y => y.BusinessEmail))
                .ForMember(x => x.BusinessPhone, o => o.MapFrom(y => y.BusinessPhone))
                .ForMember(x => x.CompanyAddress, o => o.MapFrom(y => y.CompanyAddress))
                .ForMember(x => x.AccountName, o => o.MapFrom(y => y.AccountName))
                .ForMember(x => x.AccountNumber, o => o.MapFrom(y => y.AccountNumber))
                .ReverseMap();
            CreateMap<Manager, HotelManagerDto>()
               .ForMember(x => x.ManagerId, y => y.MapFrom(z => z.AppUser.Id))
               .ForMember(x => x.DateOfBirth, y => y.MapFrom(z => z.AppUser.DateOfBirth))
               .ForMember(x => x.AccountName, y => y.MapFrom(z => z.AccountName))
               .ForMember(x => x.AccountNumber, y => y.MapFrom(z => z.AccountNumber))
               .ForMember(x => x.Avatar, y => y.MapFrom(z => z.AppUser.Avatar))
               .ForMember(x => x.BusinessEmail, y => y.MapFrom(z => z.BusinessEmail))
               .ForMember(x => x.BusinessPhone, y => y.MapFrom(z => z.BusinessPhone))
               .ForMember(x => x.CompanyAddress, y => y.MapFrom(z => z.CompanyAddress))
               .ForMember(x => x.CompanyName, y => y.MapFrom(z => z.CompanyName))
               .ForMember(x => x.CreatedAt, y => y.MapFrom(z => z.AppUser.CreatedAt))
               .ForMember(x => x.UpdatedAt, y => y.MapFrom(z => z.AppUser.UpdatedAt))
               .ForMember(x => x.FirstName, y => y.MapFrom(z => z.AppUser.FirstName))
               .ForMember(x => x.LastName, y => y.MapFrom(z => z.AppUser.LastName))
               .ForMember(x => x.IsActive, y => y.MapFrom(z => z.AppUser.IsActive))
               .ForMember(x => x.Gender, y => y.MapFrom(z => z.AppUser.Gender))
               .ForMember(x => x.TotalHotels, y => y.MapFrom(z => z.Hotels.Count))
               .ForMember(x => x.TotalAmount,
                   y => y.MapFrom(x => x.Hotels.Select(x => x.Bookings.Select(x => x.Payment.Amount).Sum()).Sum()))
               .ForMember(x => x.HotelNames, y => y.MapFrom(z => z.Hotels.Select(q => q.Name)))
               .ForMember(x => x.HotelStreet, y => y.MapFrom(z => z.Hotels.Select(q => q.Address)))
               .ForMember(x => x.HotelCity, y => y.MapFrom(z => z.Hotels.Select(q => q.City)))
               .ForMember(x => x.ManagerEmail, y => y.MapFrom(z => z.AppUser.Email))
               .ForMember(x => x.ManagerPhone, y => y.MapFrom(z => z.AppUser.PhoneNumber));

            //BedType Maps
            CreateMap<BedType, BedTypeDto>().ReverseMap();
            CreateMap<BedType,BedTypeUpdateDto>().ReverseMap();
            CreateMap<BedType,BedTypeCreateDto>().ReverseMap();

            //Image Maps
            CreateMap<Image, AddImageDto>().ReverseMap();
            CreateMap<Image,AddThumbnailDto>().ReverseMap();
            CreateMap<Image, ImageResponseDto>().ReverseMap();

            //RoomTypeBedType Maps
            CreateMap<RoomTypeBedType,AddRoomTypeBedTypeDto>()
                .ForMember(x => x.BedTypeId, y => y.MapFrom(z => z.BedTypeId))
                .ForMember(x => x.Quantity, y => y.MapFrom(z => z.Quantity))
                .ReverseMap();
            CreateMap<RoomTypeBedType, RoomTypeBedTypeDto>()
                .ForMember(x => x.BedName, y => y.MapFrom(z => z.BedType.BedName))
                .ForMember(x => x.Icon, y => y.MapFrom(z => z.BedType.Icon))
                .ReverseMap();


            //RoomType Maps
            CreateMap<RoomType, RoomTypeDto>()
                .ForMember(dest => dest.BedTypes, opt => opt.MapFrom(src => src.RoomTypeBedTypes))
                .ReverseMap();
            CreateMap<RoomTypeUpdateDto, RoomType>()
                .ForMember(dest => dest.Thumbnail, opt => opt.Condition(src => src.Thumbnail?.Length>0)).ReverseMap();
            CreateMap<RoomType,RoomTypeCreateDto>().ReverseMap();
            CreateMap<RoomType,RoomTypeResponseDto>()
                .ForMember(dest => dest.BedTypes, opt => opt.MapFrom(src => src.RoomTypeBedTypes))
                .ReverseMap();
            CreateMap<RoomType, GetAllRoomDto>()
                .ForMember(x => x.RoomTypeId, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.RoomTypeName, y => y.MapFrom(z => z.Name))
                .ForMember(x => x.Rooms, y => y.MapFrom(z => z.Rooms))
                .ReverseMap();
            CreateMap<RoomType, RoomTypeRoomsDto>()
                .ForMember(dest => dest.BedTypes, opt => opt.MapFrom(src => src.RoomTypeBedTypes))
               .ReverseMap();

            //Image Maps
            CreateMap<Image, ImageResponseDto>().ReverseMap();

            //CancellationPolicy Maps
            CreateMap<CancellationPolicy, AddCancellationPolicyDto>().ReverseMap();
            CreateMap<CancellationPolicy, CancellationPolicyUpdateDto>().ReverseMap();
            CreateMap<CancellationPolicy, CancellationPolicyDto>().ReverseMap();

            //Room Maps
            CreateMap<Room, RoomCreateDto>()
                .ForMember(x => x.CancellationPolicies, y => y.MapFrom(z => z.CancellationPolicies))
                .ReverseMap();
            CreateMap<Room, RoomDto>()
                .ForMember(x => x.CancellationPolicies, y => y.MapFrom(z => z.CancellationPolicies.OrderByDescending(c => c.MinDaysBefore)))
                .ReverseMap();
            CreateMap<Room, RoomUpdateDto>()
                .ForMember(x => x.CancellationPolicies, y => y.MapFrom(z => z.CancellationPolicies))
                .ReverseMap();

            //RoomTypeAmenity Maps
            CreateMap<RoomTypeAmenity, RoomTypeAmenityUpdateDto>().ReverseMap();

            CreateMap<Booking, CreateBookingDto>().ReverseMap();
            CreateMap<Booking, BookingResponseDto>()
                .ForMember(x => x.TotalAmount, y => y.MapFrom(src => src.Payment.Amount))
                .ForMember(x => x.RoomTypeName, y => y.MapFrom(src => src.Room.RoomType.Name))
                .ForMember(x => x.PaymentReference, y => y.MapFrom(src => src.Payment.TransactionReference))
                .ForMember(x => x.IsCancelable, y => y.MapFrom(src => src.Room.IsCancelable))
                .ForMember(x => x.BookingStatus, y => y.MapFrom(src => src.Payment.Status))
                .ForMember(x => x.MethodOfPayment, y => y.MapFrom(src => src.Payment.MethodOfPayment))
                .ReverseMap();
            CreateMap<Booking,GetAllBookingDto>()
                .ForMember(x => x.HotelName, y => y.MapFrom(z => z.Hotel.Name))
                .ForMember(x => x.HotelThumbnail, y => y.MapFrom(z => z.Hotel.Thumbnail))
                .ForMember(x => x.TotalAmount, y => y.MapFrom(z => z.Payment.Amount))
                .ForMember(x => x.BookingStatus, y => y.MapFrom(z => z.Payment.Status))
                .ReverseMap();
            CreateMap<Booking, GetBookingDto>()
               .ForMember(x => x.HotelName, y => y.MapFrom(z => z.Hotel.Name))
               .ForMember(x => x.HotelThumbnail, y => y.MapFrom(z => z.Hotel.Thumbnail))
               .ForMember(x => x.TotalAmount, y => y.MapFrom(z => z.Payment.Amount))
               .ReverseMap();
        }
    }
}
