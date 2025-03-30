using hotel_booking_dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IAddressRepository
    {
        Task<object> SearchProvincesAsync(SearchRequestDto searchRequestDto);
        Task<object> GetDistrictsByProvinceAsync(SearchRequestDto searchRequestDto);
        Task<object> GetWardsByDistrictAsync(SearchRequestDto searchRequestDto);
        Task<IEnumerable<LocationSuggestionDto>> SearchLocationsAsync(string keyword);

    }
}
