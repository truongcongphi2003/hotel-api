using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.RoomDtos;
using hotel_booking_dto.RoomTypeDtos;
using hotel_booking_models;
using hotel_booking_utilities.Pagination;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace hotel_booking_core.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<RoomDto>> CreateRoomAsync(RoomCreateDto roomCreateDto)
        {
            try
            {
                var roomType = await _unitOfWork.RoomTypes.GetRoomTypeById(roomCreateDto.RoomTypeId);
                if (roomType == null)
                    return Response<RoomDto>.Fail("Không tìm thấy loại phòng");

                var newRoom = _mapper.Map<Room>(roomCreateDto);

                await _unitOfWork.Rooms.InsertAsync(newRoom);
                await _unitOfWork.Save();

                var data = _mapper.Map<RoomDto>(newRoom);

                return Response<RoomDto>.Success("Thêm phòng thành công", data, StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return Response<RoomDto>.Fail(ex);
            }
        }

        public async Task<Response<RoomDto>> UpdateRoomAsync(string id, RoomUpdateDto roomUpdateDto)
        {
            try
            {
                var room = await _unitOfWork.Rooms.GetById(id);
                if (room == null)
                    return Response<RoomDto>.Fail("Phong khong ton tai");

                _unitOfWork.CancellationPolicys.DeleteRange(room.CancellationPolicies.ToList());

                _mapper.Map(roomUpdateDto, room);

                _unitOfWork.Rooms.Update(room);
                await _unitOfWork.Save();
                
                var data = _mapper.Map<RoomDto>(room);

                return Response<RoomDto>.Success("Cap nhat phong thanh cong", data);

            }
            catch (Exception ex)
            {
                return Response<RoomDto>.Fail(ex);
            }
        }
        
        public async Task<Response<bool>> DeleteRoomAsync(string id)
        {
            try
            {
                var room = await _unitOfWork.Rooms.GetById(id);
                if (room == null)
                    return Response<bool>.Fail("Phong khong ton tai");

                _unitOfWork.Rooms.Delete(room);
                await _unitOfWork.Save();

                return Response<bool>.Success("Xoa phong thanh cong", true);
            }
            catch (Exception ex)
            {
                return Response<bool>.Fail(ex);
            }
        }

        public async Task<Response<PageResult<IEnumerable<GetAllRoomDto>>>> GetAllRoomsAsync(string hotelId, PagingDto pagingDto)
        {
            try
            {
                var roomTypes = _unitOfWork.RoomTypes.GetAllRoomTypeRooms(hotelId);
                if(roomTypes == null)
                    return Response<PageResult<IEnumerable<GetAllRoomDto>>>.Success("Lay tat ca phong", null);

                var data = await roomTypes.PaginationAsync<RoomType, GetAllRoomDto>(pagingDto.Items, pagingDto.Page, _mapper);

                return Response<PageResult<IEnumerable<GetAllRoomDto>>>.Success("Lay tat ca phong", data);
            }
            catch (Exception ex)
            {
                return Response<PageResult<IEnumerable<GetAllRoomDto>>>.Fail(ex);
            }
        }
        public async Task<Response<RoomDto>>GetRoomByIdAsync(string id)
        {
            var room = await _unitOfWork.Rooms.GetById(id);
            if (room == null)
                return Response<RoomDto>.Fail("phòng không tồn tại");

            var data = _mapper.Map<RoomDto>(room);

            return Response<RoomDto>.Success("Lấy thông tin phòng", data);
        }
    }
}
