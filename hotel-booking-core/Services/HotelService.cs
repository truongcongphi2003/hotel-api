using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.AmenityDtos;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_dto.HotelAmenityDtos;
using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.ImageDtos;
using hotel_booking_models;
using hotel_booking_utilities.Pagination;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thda.Services;

namespace hotel_booking_core.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public HotelService(IUnitOfWork unitOfWork, IMapper mapper,IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;

        }
        public async Task<Response<HotelDetailResponseDto>> GetDetailAsync(string id)
        {
            try
            {
                var hotel = await _unitOfWork.Hotels.GetHotelByIdAsync(id);
                if (hotel == null)
                    return Response<HotelDetailResponseDto>.Fail("Khách sạn không tồn tại");

                var hotelResponse = _mapper.Map<HotelDetailResponseDto>(hotel);

                return Response<HotelDetailResponseDto>.Success($"Lấy thông tin khách sạn", hotelResponse, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Response<HotelDetailResponseDto>.Fail(ex);
            }
        }
        public async Task<Response<IEnumerable<ImageResponseDto>>> GetHotelImagesAsync(string id)
        {
            try
            {
                var images = await _unitOfWork.Hotels.GetImages(id);

                var result = _mapper.Map<IEnumerable<ImageResponseDto>>(images);

                return Response<IEnumerable<ImageResponseDto>>.Success($"Lấy ảnh khách sạn", result);
            }
            catch (Exception ex)
            {
                return Response<IEnumerable<ImageResponseDto>>.Fail(ex);
            }
        }
        public async Task<Response<HotelResponseDto>> GetHotelAsync(string hotelId)
        {
            try
            {
                var hotel = await _unitOfWork.Hotels.GetByIdAsync(hotelId);
                if (hotel == null)
                    return Response<HotelResponseDto>.Fail("Khách sạn không tồn tại");
     
                var hotelResponse = _mapper.Map<HotelResponseDto>(hotel);

                return Response<HotelResponseDto>.Success($"Lấy thông tin khách sạn", hotelResponse, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Response<HotelResponseDto>.Fail(ex);
            }
        }

        public async Task<Response<HotelResponseDto>> CreateHotelAsync(string managerId, CreateHotelDto dto)
        {
            try
            {
                var hotel = _mapper.Map<Hotel>(dto);
                hotel.ManagerId = managerId;

                await _unitOfWork.Hotels.InsertAsync(hotel);
                await _unitOfWork.Save();

                var hotelResponse = _mapper.Map<HotelResponseDto>(hotel);

                return Response<HotelResponseDto>.Success($"Thêm khách sạn {hotel.Name} thành công", hotelResponse, StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return Response<HotelResponseDto>.Fail(ex);
            }
        }

        public async Task<Response<HotelResponseDto>> UpdateHotelAsync(string hotelId, UpdateHotelDto dto)
        {
            try
            {
                var hotel = await _unitOfWork.Hotels.GetByIdAsync(hotelId);
                if (hotel == null)
                    return Response<HotelResponseDto>.Fail("Khách sạn không tồn tại");

                hotel.Name = dto.Name;
                hotel.Description = dto.Description;
                hotel.StarRating = dto.StarRating;
                hotel.Email = dto.Email;
                hotel.Phone = dto.Phone;
                hotel.Address = dto.Address;
                hotel.City = dto.City;
                hotel.ProvinceCode = dto.ProvinceCode;
                hotel.DistrictCode = dto.DistrictCode;
                hotel.WardCode = dto.WardCode;
                hotel.MapLocation = dto.MapLocation;

                if (dto.IsDeleteThumbnail && hotel.Thumbnail != null)
                {
                    await _imageService.DeleteResourcesAsync(hotel.Thumbnail);
                    hotel.Thumbnail = null;
                }
                if (dto.Thumbnail!=null && dto.Thumbnail.Length > 0)
                {
                    var thumbnail = await _imageService.UploadImageAsync(dto.Thumbnail);
                    if (thumbnail == null || string.IsNullOrEmpty(thumbnail.SecureUrl?.ToString()))
                    {
                        return Response<HotelResponseDto>.Fail("Upload ảnh thất bại, vui lòng thử lại.");
                    }
                    hotel.Thumbnail = thumbnail.SecureUrl.ToString();
                }
                

                _unitOfWork.Hotels.Update(hotel);
                await _unitOfWork.Save();

                var hotelResponse = _mapper.Map<HotelResponseDto>(hotel);

                return Response<HotelResponseDto>.Success("Cập nhật thông tin khách sạn thành công", hotelResponse);

            }
            catch (Exception ex)
            {
                return Response<HotelResponseDto>.Fail(ex);
            }
        }

        public async Task<Response<string>> UpdateHotelThumbnail(string hotelId, AddThumbnailDto dto)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(hotelId);
            if (hotel == null)
                return Response<string>.Fail("Khách sạn không tồn tại");

            

            var thumbnail = await _imageService.UploadImageAsync(dto.Thumbnail);
            if (thumbnail == null || string.IsNullOrEmpty(thumbnail.SecureUrl?.ToString()))
            {
                return Response<string>.Fail("Upload ảnh thất bại, vui lòng thử lại.");
            }

            if (!string.IsNullOrEmpty(hotel.Thumbnail))
            {
                await _imageService.DeleteResourcesAsync(hotel.Thumbnail);
            }
            hotel.Thumbnail = thumbnail.SecureUrl.ToString();

            _unitOfWork.Hotels.Update(hotel);
            await _unitOfWork.Save();

            return Response<string>.Success("Cập nhật ảnh chính khách sạn thành công",hotel.Thumbnail);

        }
        public async Task<Response<IEnumerable<ImageResponseDto>>> GetHotelImages(string hotelId)
        {
            try
            {
                var hotel = await _unitOfWork.Hotels.GetByIdAsync(hotelId);
                if (hotel == null)
                    return Response<IEnumerable<ImageResponseDto>>.Fail("Khách sạn không tồn tại");
                var images = hotel.Images.ToList();

                var data = _mapper.Map<IEnumerable<ImageResponseDto>>(images);

                return Response<IEnumerable<ImageResponseDto>>.Success("Cập nhật ảnh khách sạn thành công", data);
            }
            catch (Exception ex)
            {
                return Response<IEnumerable<ImageResponseDto>>.Fail(ex);
            }
        }

        public async Task<Response<IEnumerable<ImageResponseDto>>> UpdateHotelImages(string hotelId, AddImageDto dto)
        {
            try
            {
                var hotel = await _unitOfWork.Hotels.GetByIdAsync(hotelId);
                if (hotel == null)
                    return Response<IEnumerable<ImageResponseDto>>.Fail("Khách sạn không tồn tại");

                var images = new List<Image>();

                if (dto.Files != null)
                {
                    var uploads = await _imageService.UploadImagesAsync(dto.Files);
                    foreach (var upload in uploads)
                    {
                        var image = new Image()
                        {
                            ImageUrl = upload.SecureUrl.ToString(),
                        };
                        hotel.Images.Add(image);
                        images.Add(image);
                    }
                }

                if (dto.ImageRemoveIds != null)
                {
                    foreach (var imageId in dto.ImageRemoveIds)
                    {
                        var image = hotel.Images.FirstOrDefault(h => h.Id == imageId);
                        if (image == null)
                            continue;
                        hotel.Images.Remove(image);
                        await _imageService.DeleteResourcesAsync(image.ImageUrl);
                    }
                }

                _unitOfWork.Hotels.Update(hotel);
                await _unitOfWork.Save();

                var data = _mapper.Map<IEnumerable<ImageResponseDto>>(images);

                return Response<IEnumerable<ImageResponseDto>>.Success("Cập nhật ảnh khách sạn thành công", data);
            }
            catch (Exception ex)
            {
                return Response<IEnumerable<ImageResponseDto>>.Fail(ex);
            }
        }

        public async Task<Response<bool>> UpdateHotelAmenities(string hotelId, HotelAmenityUpdateDto dto)
        {
            try
            {
                var hotel = await _unitOfWork.Hotels.GetByIdAsync(hotelId);
                if (hotel == null)
                    return Response<bool>.Fail("Khách sạn không tồn tại");

                //if (dto.RemoveAmenityIds != null && dto.RemoveAmenityIds.Any())
                //{
                //    foreach (var amenityId in dto.RemoveAmenityIds)
                //    {
                //        var amenity = hotel.HotelAmenities.FirstOrDefault(h => h.AmenityId == amenityId);

                //        if (amenity != null)
                //        {
                //            hotel.HotelAmenities.Remove(amenity);
                //        }
                //    }
                //}
                hotel.HotelAmenities.Clear();

                if (dto.UpdateAmenityIds != null && dto.UpdateAmenityIds.Any())
                {
                    foreach (var amenityId in dto.UpdateAmenityIds)
                    {
                        var amenity = await _unitOfWork.Amenities.GetByIdAsync(amenityId);
                        if (amenity != null)
                        {
                            var newHotelAmenity = new HotelAmenity()
                            {
                                HotelId = hotel.Id,
                                AmenityId = amenityId,
                            };
                            hotel.HotelAmenities.Add(newHotelAmenity);
                        }
                    }
                }
                await _unitOfWork.Save();

                return Response<bool>.Success("Cập nhật tiện ích thành công", true);
            }
            catch (Exception ex) {
                return Response<bool>.Fail(ex);
            }
        }

        public async Task<Response<PageResult<IEnumerable<GetAllHotelDto>>>> GetHotelsByLocation(LocationRequestDto locationRequestDto, PagingDto pagingDto)
        {
            try
            {
                var query = _unitOfWork.Hotels.GetByLocation(locationRequestDto);
                var result = await query.PaginationAsync<Hotel, GetAllHotelDto>(pageSize: pagingDto.Items, pageNumber: pagingDto.Page, mapper: _mapper);

                return Response<PageResult<IEnumerable<GetAllHotelDto>>>.Success("Lấy dữ liệu khách sạn",result);
            }
            catch (Exception ex)
            {
                return Response<PageResult<IEnumerable<GetAllHotelDto>>>.Fail(ex);
            }
        }

    }
}
