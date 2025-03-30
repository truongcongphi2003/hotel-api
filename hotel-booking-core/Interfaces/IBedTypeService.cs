using hotel_booking_dto;
using hotel_booking_dto.BedTypeDtos;
using hotel_booking_models;
using hotel_booking_utilities.Pagination;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IBedTypeService
    {
        Task<Response<BedTypeDto>> CreateBedTypeAsync(string hotelId, BedTypeCreateDto dto);
        Task<Response<BedTypeDto>> UpdateBedTypeAsync(string hotelId, string id, BedTypeUpdateDto dto);
        Task<Response<bool>> DeleteBedTypeAsync(string bedTypeId);
        Task<Response<PageResult<IEnumerable<BedTypeDto>>>> GetAllBedType(string hotelId, PagingDto pagingDto);
        Task<Response<BedTypeDto>> GetBedTypeById(string id);
        Task<Response<IEnumerable<BedTypeDto>>> SearchBedTypeAsync(SearchRequestDto request);
    }
}
