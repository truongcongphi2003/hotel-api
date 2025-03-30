using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto.ManagerDtos;
using hotel_booking_dto;
using hotel_booking_models;
using hotel_booking_utilities.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hotel_booking_dto.ManagerRequestDtos;

namespace hotel_booking_core.Interfaces
{
    public interface IManagerService
    {
        Task<Response<PageResult<IEnumerable<HotelManagerDto>>>> GetAllHotelManagersAsync(PagingDto paging);
        Task<Response<string>> AddManagerRequestAsync(AddManagerRequestDto managerRequest);
        Task<Response<bool>> ApproveManagerRequestAsync(string email);
        Task<Response<bool>> RegisterManagerAsync(RegisterManagerDto registerDto);
        Task<Response<PageResult<IEnumerable<ManagerRequestResponseDto>>>> GetAllManagerRequest(PagingDto paging);
        Task<Response<bool>> SendManagerInvite(string email);
        Task<Response<bool>> CheckTokenExpiring(string email, string token);


    }
}
