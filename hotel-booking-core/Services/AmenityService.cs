using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.Contexts;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.AmenityDtos;
using hotel_booking_models;
using hotel_booking_utilities.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AmenityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<AmenityResponseDto>> CreateAmenityAsync(AmenityCreateDto dto)
        {
            try
            {
                var amenity = _mapper.Map<Amenity>(dto);
                await _unitOfWork.Amenities.InsertAsync(amenity);
                await _unitOfWork.Save();

                var data = _mapper.Map<AmenityResponseDto>(amenity);

                return Response<AmenityResponseDto>.Success("Tạo tiện ích thành công", data, StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return Response<AmenityResponseDto>.Fail($"Lỗi xảy ra: {ex.Message}");
            }
        }

        public async Task<Response<AmenityUpdateDto>> UpdateAmenityAsync(string id, AmenityUpdateDto dto)
        {
            try
            {
                var existingAmenity = await _unitOfWork.Amenities.GetByIdAsync(id);
                if (existingAmenity == null)
                    return Response<AmenityUpdateDto>.Fail("Không tìm thấy tiện ích");

                _mapper.Map(dto, existingAmenity);
                _unitOfWork.Amenities.Update(existingAmenity);
                await _unitOfWork.Save();

                var data = _mapper.Map<AmenityUpdateDto>(existingAmenity);

                return Response<AmenityUpdateDto>.Success("Cập nhật tiện ích thành công", data, StatusCodes.Status200OK);

            }
            catch (Exception ex)
            {
                return Response<AmenityUpdateDto>.Fail($"Lỗi xảy ra: {ex.Message}");
            }
        }

        public async Task<Response<string>> DeleteAmenityAsync(string id)
        {
            try
            {
                var existingAmenity = await _unitOfWork.Amenities.GetByIdAsync(id);
                if (existingAmenity == null)
                    return Response<string>.Fail("Không tìm thấy tiện ích");

                _unitOfWork.Amenities.Delete(existingAmenity);
                await _unitOfWork.Save();

                return Response<string>.Success(null, $"Đã xóa tiện ích: {existingAmenity.Name} thành công");
            }
            catch (Exception ex)
            {
                return Response<string>.Fail($"Lỗi xảy ra: {ex.Message}");
            }
        }

        private IEnumerable<AmenityDto> BuildAmenityTree(IEnumerable<AmenityDto> amenities)
        {
            var lookup = amenities
                .GroupBy(a => a.ParentId??"")
                .ToDictionary(g => g.Key!, g => g.ToList());

            IEnumerable<AmenityDto> BuildTree(string? parentId)
            {
                if (!lookup.TryGetValue(parentId??"", out var children))
                    return Enumerable.Empty<AmenityDto>();

                return children.Select(a => new AmenityDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Icon = a.Icon,
                    ParentId = a.ParentId,
                    Children = BuildTree(a.Id).Any() ? BuildTree(a.Id) : null
                });
            }

            //bắt đầu từ gốc ParentId = null
            return BuildTree(null);
        }

        public async Task<Response<PageResult<IEnumerable<AmenityDto>>>> GetAllAmenitiesAsync(SearchRequestDto searchRequestDto, PagingDto pagingDto)
        {
            try
            {
                var amenities = _unitOfWork.Amenities.GetAll(searchRequestDto);

                var result = await amenities.PaginationAsync<Amenity, AmenityDto>(pageSize: pagingDto.Items, pageNumber: pagingDto.Page, mapper: _mapper);

                result.PageItems = BuildAmenityTree(result.PageItems);

                return Response<PageResult<IEnumerable<AmenityDto>>>.Success("Lấy danh sách tiện ích thành công", result, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Response<PageResult<IEnumerable<AmenityDto>>>.Fail($"Lỗi xảy ra: {ex.Message}");
            }
        }

        public async Task<Response<AmenityResponseDto>> GetAmenityByIdAsync(string id)
        {
            try
            {
                var amenity = await _unitOfWork.Amenities.GetByIdAsync(id);
                if (amenity == null)
                    return Response<AmenityResponseDto>.Fail("Không tìm thấy tiện ích");

                var data = _mapper.Map<AmenityResponseDto>(amenity);
                return Response<AmenityResponseDto>.Success("Lấy thông tin tiện ích thành công", data);
            }
            catch (Exception ex)
            {
                return Response<AmenityResponseDto>.Fail($"Lỗi xảy ra: {ex.Message}");
            }
        }

        public async Task<Response<IEnumerable<AmenityResponseDto>>> SearchAmenitiesAsync(SearchRequestDto request)
        {
            try
            {
                var query = _unitOfWork.Amenities.Search(request);
                var amenities = await query.ToListAsync();
                //if(amenities == null || !amenities.Any())
                //    return Response<IEnumerable<AmenityResponseDto>>.Fail($"Không có tiện ích nào");

                var data = _mapper.Map<IEnumerable<AmenityResponseDto>>(amenities);

                return Response<IEnumerable<AmenityResponseDto>>.Success("Tìm kiếm tiện ích thành công", data);
            }
            catch (Exception ex)
            {
                return Response<IEnumerable<AmenityResponseDto>>.Fail($"Lỗi xảy ra: {ex.Message}");
            }
        }

        public async Task<Response<PageResult<IEnumerable<AmenityDto>>>> GetAllAmenitiesByHotelIdAsync(string hotelId, SearchRequestDto searchRequestDto, PagingDto pagingDto)
        {
            try
            {
                var hotelAmenities = _unitOfWork.Amenities.GetAllHotelAmenities(hotelId,searchRequestDto);

                var result = await hotelAmenities.PaginationAsync<Amenity, AmenityDto>(pageSize: pagingDto.Items, pageNumber: pagingDto.Page, mapper: _mapper);

                result.PageItems = BuildAmenityTree(result.PageItems);

                return Response<PageResult<IEnumerable<AmenityDto>>>.Success("Lấy danh sách tiện ích khách sạn thành công", result, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Response<PageResult<IEnumerable<AmenityDto>>>.Fail(ex);
            }
        }
        public async Task<Response<PageResult<IEnumerable<AmenityDto>>>> GetAllAmenitiesByRoomTypeIdAsync(string roomTypeId, PagingDto pagingDto)
        {
            try
            {
                var roomTypeAmenities = _unitOfWork.Amenities.GetAllRoomTypeAmenities(roomTypeId);

                var result = await roomTypeAmenities.PaginationAsync<Amenity, AmenityDto>(pageSize: pagingDto.Items, pageNumber: pagingDto.Page, mapper: _mapper);

                result.PageItems = BuildAmenityTree(result.PageItems);

                return Response<PageResult<IEnumerable<AmenityDto>>>.Success("Lấy danh sách tiện ích loại phòng thành công", result, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Response<PageResult<IEnumerable<AmenityDto>>>.Fail(ex);
            }
        }
    }
}
