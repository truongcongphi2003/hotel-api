﻿using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IRoomTypeImageRepository : IGenericRepository<RoomTypeImage>
    {
        IQueryable<RoomTypeImage> GetAllByRoomTypeId(string roomTypeId);
    }
}
