using hotel_booking_core.Interfaces;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        // 🔹 Tìm kiếm tỉnh/thành phố
        public async Task<object> SearchProvincesAsync(SearchRequestDto searchRequestDto)
        {
            try
            {
                return await _addressRepository.SearchProvincesAsync(searchRequestDto);
            }
            catch (Exception)
            {
                return Enumerable.Empty<object>();
            }
        }

        // 🔹 Tìm kiếm quận/huyện
        public async Task<object> SearchDistrictsByProvinceAsync(SearchRequestDto searchRequestDto)
        {
            try
            {
                return await _addressRepository.GetDistrictsByProvinceAsync(searchRequestDto);
            }
            catch (Exception)
            {
                return Enumerable.Empty<object>();
            }
        }

        // 🔹 Tìm kiếm phường/xã
        public async Task<object> SearchWardsByDistrictAsync(SearchRequestDto searchRequestDto)
        {
            try
            {
                return await _addressRepository.GetWardsByDistrictAsync(searchRequestDto);
            }
            catch (Exception)
            {
                return Enumerable.Empty<object>();
            }
        }
        public async Task<Response<IEnumerable<LocationSuggestionDto>>> SearchLocationsAsync(string keyword)
        {
            try
            {
                var internalLocations =  await _addressRepository.SearchLocationsAsync(keyword);
                var externalLocations = await SearchLocationsFromOSMAsync(keyword);
                var allLocations = internalLocations.Concat(externalLocations).ToList();
                return Response<IEnumerable< LocationSuggestionDto >>.Success("Lấy địa điểm, khách sạn gợi ý", allLocations, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Response<IEnumerable<LocationSuggestionDto>>.Fail(ex);
            }
        }
        private async Task<List<LocationSuggestionDto>> SearchLocationsFromOSMAsync(string keyword)
        {
            string url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(keyword)}&countrycodes=VN&limit=10";

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("HotelBk/1.0");

            var response = await httpClient.GetStringAsync(url);
            var results = JsonConvert.DeserializeObject<List<OSMSearchResultDto>>(response);

            return results.Select(r => new LocationSuggestionDto
            {
                Type = "External",
                FullNameEn = "External",
                Name = r.DisplayName,
                Latitude = r.Lat,
                Longitude = r.Lon
            }).ToList();
        }

    }
}
