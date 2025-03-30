using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.BedTypeDtos;
using hotel_booking_dto.HotelAmenityDtos;
using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.ImageDtos;
using hotel_booking_dto.RoomTypeAmenityDtos;
using hotel_booking_dto.RoomTypeDtos;
using hotel_booking_models;
using hotel_booking_utilities.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thda.Services;

namespace hotel_booking_core.Services
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public RoomTypeService(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }
        public async Task <Response<IEnumerable<RoomTypeRoomsDto>>> SearchAvailableRooms(SearchHotelDto searchDto)
        {
            try
            {
                var roomTypes = await _unitOfWork.RoomTypes.SearchAvailableRooms(searchDto);
                var result = _mapper.Map<IEnumerable<RoomTypeRoomsDto>>(roomTypes);
                return Response<IEnumerable<RoomTypeRoomsDto>>.Success("Lọc phòng", result);
            }catch(Exception ex)
            {
                return Response<IEnumerable<RoomTypeRoomsDto>>.Fail(ex);
            }
        }

        public async Task<Response<RoomTypeResponseDto>> CreateRoomTypeAsync(string hotelId, RoomTypeCreateDto dto)
        {
            try
            {
                var hotel = await _unitOfWork.Hotels.GetByIdAsync(hotelId);
                if (hotel == null)
                    return Response<RoomTypeResponseDto>.Fail("Khách sạn không tồn tại");

                var roomType = _mapper.Map<RoomType>(dto);
                roomType.HotelId = hotelId;
                if (dto.Thumbnail != null && dto.Thumbnail.Length > 0)
                {
                    var thumbnail = await _imageService.UploadImageAsync(dto.Thumbnail);
                    if (thumbnail == null || string.IsNullOrEmpty(thumbnail.SecureUrl.ToString()))
                    {
                        return Response<RoomTypeResponseDto>.Fail("Upload ảnh thất bại, vui lòng thử lại.");
                    }

                    hotel.Thumbnail = thumbnail.SecureUrl.ToString();
                }

                await _unitOfWork.RoomTypes.InsertAsync(roomType);

                if (dto.BedTypes != null && dto.BedTypes.Any())
                {
                    foreach (var bedType in dto.BedTypes)
                    {
                        var roomTypeBed = new RoomTypeBedType()
                        {
                            RoomTypeId = roomType.Id,
                            BedTypeId = bedType.BedTypeId,
                            Quantity = bedType.Quantity
                        };
                        roomType.RoomTypeBedTypes.Add(roomTypeBed);
                    }
                }
                await _unitOfWork.Save();

                var data = _mapper.Map<RoomTypeResponseDto>(roomType);
                return Response<RoomTypeResponseDto>.Success("Thêm loại phòng thành công", data, StatusCodes.Status201Created);
            }
            catch(Exception ex)
            {
                return Response<RoomTypeResponseDto>.Fail(ex);

            }
        }
        

        public async Task<Response<RoomTypeResponseDto>> UpdateRoomTypeAsync(string hotelId, string id, RoomTypeUpdateDto dto)
        {
            var roomType = await _unitOfWork.RoomTypes.GetRoomTypeById(id);
            if (roomType == null)
                return Response<RoomTypeResponseDto>.Fail("Loại phòng không tồn tại");

            _mapper.Map(dto,roomType);
            if (dto.IsDeleteThumbnail && roomType.Thumbnail != null)
            {
                await _imageService.DeleteResourcesAsync(roomType.Thumbnail);
                roomType.Thumbnail = null;
            }
            if (dto.Thumbnail != null && dto.Thumbnail.Length > 0)
            {
                var thumbnail = await _imageService.UploadImageAsync(dto.Thumbnail);
                if (thumbnail == null || string.IsNullOrEmpty(thumbnail.SecureUrl?.ToString()))
                {
                    return Response<RoomTypeResponseDto>.Fail("Upload ảnh thất bại, vui lòng thử lại.");
                }

                roomType.Thumbnail = thumbnail.SecureUrl.ToString();
            }
            if (dto.BedTypes != null && dto.BedTypes.Any())
            {
                var existingBedTypes = roomType.RoomTypeBedTypes.ToList();

                foreach (var bedTypeDto in dto.BedTypes)
                {
                    var existingBed = existingBedTypes.FirstOrDefault(b => b.BedTypeId == bedTypeDto.BedTypeId);

                    if (existingBed != null)
                    {
                        existingBed.Quantity = bedTypeDto.Quantity;
                    }
                    else
                    {
                        roomType.RoomTypeBedTypes.Add(new RoomTypeBedType
                        {
                            RoomTypeId = roomType.Id,
                            BedTypeId = bedTypeDto.BedTypeId,
                            Quantity = bedTypeDto.Quantity
                        });
                    }
                }
                var bedTypeIdsToKeep = dto.BedTypes.Select(b => b.BedTypeId).ToList();
                var bedsToRemove = existingBedTypes.Where(b => !bedTypeIdsToKeep.Contains(b.BedTypeId)).ToList();
                foreach (var bedToRemove in bedsToRemove)
                {
                    roomType.RoomTypeBedTypes.Remove(bedToRemove);
                }
            }
            _unitOfWork.RoomTypes.Update(roomType);
            await _unitOfWork.Save();

            var data = _mapper.Map<RoomTypeResponseDto>(roomType);

            return Response<RoomTypeResponseDto>.Success("Cập nhật loại phòng thành công", data);
        }

        public async Task<Response<bool>> DeleteRoomTypeAsync(string id)
        {
            var roomType = await _unitOfWork.RoomTypes.GetRoomTypeById(id);
            if (roomType == null)
                return Response<bool>.Fail("Loại phòng không tồn tại");
            
            _unitOfWork.RoomTypes.Delete(roomType);
            await _unitOfWork.Save();
            return Response<bool>.Success("Xóa loại phòng thành công",true);
        }

        public async Task<Response<PageResult<IEnumerable<RoomTypeDto>>>> GetAllRoomTypes(string hotelId, SearchRequestDto searchRequestDto, PagingDto pagingDto)
        {
            var roomTypes = _unitOfWork.RoomTypes.GetAllHotelRoomType(hotelId,searchRequestDto);

            var data = await roomTypes.PaginationAsync<RoomType, RoomTypeDto>(pagingDto.Items, pagingDto.Page, _mapper);

            return Response<PageResult<IEnumerable<RoomTypeDto>>>.Success("Lấy tất cả loại phòng", data);
        } 

        public async Task<Response<RoomTypeResponseDto>> GetRoomTypeById(string id)
        {
            var roomType = await _unitOfWork.RoomTypes.GetRoomTypeById(id);
            if (roomType == null)
                return Response<RoomTypeResponseDto>.Fail("Loại phòng không tồn tại");

            var data = _mapper.Map<RoomTypeResponseDto>(roomType);

            return Response<RoomTypeResponseDto>.Success("Lấy thông tin loại phòng", data);
        }

        public async Task<Response<string>> UpdateRoomTypeThumbnail(string id, AddThumbnailDto dto)
        {
            var roomType = await _unitOfWork.RoomTypes.GetRoomTypeById(id);
            if (roomType == null)
                return Response<string>.Fail("Loại phòng không tồn tại");

            var thumbnail = await _imageService.UploadImageAsync(dto.Thumbnail);
            if (thumbnail == null || string.IsNullOrEmpty(thumbnail.SecureUrl?.ToString()))
            {
                return Response<string>.Fail("Upload ảnh thất bại, vui lòng thử lại.");
            }

            if (!string.IsNullOrEmpty(roomType.Thumbnail))
            {
                await _imageService.DeleteResourcesAsync(roomType.Thumbnail);
            }
            roomType.Thumbnail = thumbnail.SecureUrl.ToString();

            _unitOfWork.RoomTypes.Update(roomType);
            await _unitOfWork.Save();

            return Response<string>.Success("Cập nhật ảnh chính loại phòng thành công", roomType.Thumbnail);
        }

        public async Task<Response<IEnumerable<ImageResponseDto>>> UpdateRoomTypeImages(string id, AddImageDto dto)
        {
            try
            {
                var roomType = await _unitOfWork.RoomTypes.GetRoomTypeById(id);
                if (roomType == null)
                    return Response<IEnumerable<ImageResponseDto>>.Fail("Loại phòng không tồn tại");

                var images = new List<Image>();

                if (dto.Files != null&& dto.Files.Any())
                {
                    var uploads = await _imageService.UploadImagesAsync(dto.Files);
                    foreach (var upload in uploads)
                    {
                        var image = new Image()
                        {
                            ImageUrl = upload.SecureUrl.ToString(),
                        };
                        roomType.Hotel.Images.Add(image);
                        await _unitOfWork.Save();

                        var roomTypeImage = new RoomTypeImage()
                        {
                            RoomTypeId = roomType.Id,
                            ImageId = image.Id,
                        };
                        roomType.RoomTypeImages.Add(roomTypeImage);
                        images.Add(image);
                    }
                }

                if (dto.ImageRemoveIds != null && dto.ImageRemoveIds.Any())
                {
                    foreach (var imageId in dto.ImageRemoveIds)
                    {
                        var image = roomType.Hotel.Images.FirstOrDefault(h => h.Id == imageId);
                        if (image == null)
                            continue;
                        roomType.Hotel.Images.Remove(image);
                        await _imageService.DeleteResourcesAsync(image.ImageUrl);
                    }
                }

                _unitOfWork.RoomTypes.Update(roomType);
                await _unitOfWork.Save();

                var data = _mapper.Map<IEnumerable<ImageResponseDto>>(images);

                return Response<IEnumerable<ImageResponseDto>>.Success("Cập nhật ảnh loại phòng thành công", data);
            }
            catch (Exception ex)
            {
                return Response<IEnumerable<ImageResponseDto>>.Fail(ex);
            }
        }

        public async Task<Response<GetRoomTypeImageDto>> GetImageRoomTypeAsync(string id)
        {
            var roomTypeImages = _unitOfWork.RoomTypeImages.GetAllByRoomTypeId(id);

            var images = await roomTypeImages.Where(r => r.RoomTypeId == id).Select(r => r.Image).ToListAsync();
            var imageDtos = _mapper.Map<IEnumerable<ImageResponseDto>>(images);

            var data = new GetRoomTypeImageDto()
            {
                Thumbnail = roomTypeImages.Select(r => r.RoomType.Thumbnail).FirstOrDefault(),
                Images = imageDtos
            };

            return Response<GetRoomTypeImageDto>.Success("Lấy ảnh chính và các ảnh loại phòng",data);
        }
        public async Task<Response<IEnumerable<string>>> GetImageListAsync(string id)
        {
            var roomTypeImages = _unitOfWork.RoomTypeImages.GetAllByRoomTypeId(id);

            var images = await roomTypeImages
                .Where(r => r.RoomTypeId == id)
                .Select(r => new { ImageUrl = r.Image.ImageUrl, Thumbnail = r.RoomType.Thumbnail })
                .ToListAsync();

            if (!images.Any())
                return Response<IEnumerable<string>>.Success("Lấy tất cả ảnh loại phòng", null);

            var data = new List<string>();

            var thumbnail = images.First().Thumbnail;
            if (!string.IsNullOrEmpty(thumbnail))
                data.Add(thumbnail);

            data.AddRange(images.Select(r => r.ImageUrl));

            return Response<IEnumerable<string>>.Success("Lấy tất cả ảnh loại phòng",data);
        }

        public async Task<Response<bool>> UpdateRoomTypeAmenities(string id, RoomTypeAmenityUpdateDto dto)
        {
            try
            {
                var roomType = await _unitOfWork.RoomTypes.GetRoomTypeById(id);
                if (roomType == null)
                    return Response<bool>.Fail("Loại phòng không tồn tại");

                if (dto.RemoveAmenityIds != null && dto.RemoveAmenityIds.Any())
                {
                    foreach (var amenityId in dto.RemoveAmenityIds)
                    {
                        var amenity = roomType.RoomTypeAmenities.FirstOrDefault(h => h.AmenityId == amenityId);

                        if (amenity != null)
                        {
                            roomType.RoomTypeAmenities.Remove(amenity);
                        }
                    }
                }

                if (dto.AddAmenityIds != null && dto.AddAmenityIds.Any())
                {
                    foreach (var amenityId in dto.AddAmenityIds)
                    {
                        var amenity = await _unitOfWork.Amenities.GetByIdAsync(amenityId.Id);
                        if (amenity != null)
                        {
                            var newRoomAmenity = new RoomTypeAmenity()
                            {
                                RoomTypeId = roomType.Id,
                                AmenityId = amenityId.Id,
                                IsMain = amenityId.IsMain
                            };
                            roomType.RoomTypeAmenities.Add(newRoomAmenity);
                        }
                    }
                }
                await _unitOfWork.Save();

                return Response<bool>.Success("Cập nhật tiện ích phòng thành công", true);
            }
            catch (Exception ex)
            {
                return Response<bool>.Fail(ex);
            }
        }
        public async Task<Response<IEnumerable<RoomTypeDto>>> SearchRoomTypeAsync(SearchRequestDto request)
        {
            try
            {
                var query = _unitOfWork.RoomTypes.Search(request);
                var bedTypes = await query.ToListAsync();           

                var data = _mapper.Map<IEnumerable<RoomTypeDto>>(bedTypes);

                return Response<IEnumerable<RoomTypeDto>>.Success("Tìm kiếm Loại phòng", data);
            }
            catch (Exception ex)
            {
                return Response<IEnumerable<RoomTypeDto>>.Fail(ex);
            }
        }

    }
}
