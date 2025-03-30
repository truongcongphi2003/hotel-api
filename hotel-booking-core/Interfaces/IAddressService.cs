using hotel_booking_dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IAddressService
    {
        Task<object> SearchProvincesAsync(SearchRequestDto searchRequestDto);
        Task<object> SearchDistrictsByProvinceAsync(SearchRequestDto searchRequestDto);
        Task<object> SearchWardsByDistrictAsync(SearchRequestDto searchRequestDto);
        Task<Response<IEnumerable<LocationSuggestionDto>>> SearchLocationsAsync(string keyword);
    }
}
