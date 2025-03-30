using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.AmenityDtos;
using hotel_booking_dto.BedTypeDtos;
using hotel_booking_models;
using hotel_booking_utilities.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace hotel_booking_core.Services
{
    public class BedTypeService : IBedTypeService
    { 
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BedTypeService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<BedTypeDto>> CreateBedTypeAsync(string hotelId, BedTypeCreateDto dto)
        {
            try
            {
                //var checkAuth = await _unitOfWork.Hotels.CheckHotelManager(hotelId, managerId);
                //if (!checkAuth)
                //    return Response<BedTypeDto>.Fail("Bạn không có quyền thao tác trên khách sạn này", StatusCodes.Status401Unauthorized);

                var bedType = _mapper.Map<BedType>(dto);
                bedType.HotelId = hotelId;

                await _unitOfWork.BedTypes.InsertAsync(bedType);
                await _unitOfWork.Save();

                var response = _mapper.Map<BedTypeDto>(bedType);

                return Response<BedTypeDto>.Success("Thêm loại giường thành công", response, StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return Response<BedTypeDto>.Fail(ex);
            }
        }

        public async Task<Response<BedTypeDto>> UpdateBedTypeAsync(string hotelId,string id, BedTypeUpdateDto dto)
        {
            try
            {
                //var checkAuth = await _unitOfWork.Hotels.CheckHotelManager(hotelId, managerId);
                //if (!checkAuth)
                //    return Response<BedTypeDto>.Fail("Bạn không có quyền thao tác trên khách sạn này", StatusCodes.Status401Unauthorized);

                var bedType = await _unitOfWork.BedTypes.GetById(id);
                if(bedType == null)
                    return Response<BedTypeDto>.Fail("Loại giường không tồn tại", StatusCodes.Status404NotFound);

                _mapper.Map(dto,bedType);

                _unitOfWork.BedTypes.Update(bedType);
                await _unitOfWork.Save();

                var data = _mapper.Map<BedTypeDto>(bedType);

                return Response<BedTypeDto>.Success("Cập nhật loại giường thành công", data, StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return Response<BedTypeDto>.Fail(ex);
            }
        }

        public async Task<Response<bool>> DeleteBedTypeAsync(string bedTypeId)
        {
            try
            {
                var bedType = await _unitOfWork.BedTypes.GetById(bedTypeId);
                if (bedType == null)
                    return Response<bool>.Fail("Loại giường không tồn tại", StatusCodes.Status404NotFound);

                _unitOfWork.BedTypes.Delete(bedType);
                await _unitOfWork.Save();
                return Response<bool>.Success("Xóa loại giường thành công", true);

            }
            catch (Exception ex)
            {
                return Response<bool>.Fail(ex);
            }
        }

        public async Task<Response<PageResult<IEnumerable<BedTypeDto>>>> GetAllBedType(string hotelId, PagingDto pagingDto)
        {
            try
            {
                var bedTypes = _unitOfWork.BedTypes.GetAllByHotelId(hotelId);

                var data = await bedTypes.PaginationAsync<BedType, BedTypeDto>(pagingDto.Items, pagingDto.Page, _mapper);

                return Response<PageResult<IEnumerable<BedTypeDto>>>.Success("Tất cả loại giường", data);
            }
            catch (Exception ex)
            {
                return Response<PageResult<IEnumerable<BedTypeDto>>>.Fail(ex);
            }
        }
        public async Task<Response<IEnumerable<BedTypeDto>>> SearchBedTypeAsync(SearchRequestDto request)
        {
            try
            {
                var query = _unitOfWork.BedTypes.Search(request);
                var bedTypes = await query.ToListAsync();
                //if(amenities == null || !amenities.Any())
                //    return Response<IEnumerable<AmenityResponseDto>>.Fail($"Không có tiện ích nào");

                var data = _mapper.Map<IEnumerable<BedTypeDto>>(bedTypes);

                return Response<IEnumerable<BedTypeDto>>.Success("Tìm kiếm loại giường", data);
            }
            catch (Exception ex)
            {
                return Response<IEnumerable<BedTypeDto>>.Fail($"Lỗi xảy ra: {ex.Message}");
            }
        }
        public async Task<Response<BedTypeDto>> GetBedTypeById(string id)
        {
            try
            {
                var bedType = await _unitOfWork.BedTypes.GetById(id);
                if (bedType == null)
                    return Response<BedTypeDto>.Fail("Loại phòng không tồn tại");

                var data = _mapper.Map<BedTypeDto>(bedType);

                return Response<BedTypeDto>.Success("Lấy loại phòng thành công", data);
            }
            catch (Exception ex) {
                return Response<BedTypeDto>.Fail(ex);
            }
        }
    }
}
