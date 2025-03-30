using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class AddressRepository : IAddressRepository
    {
        private readonly HotelContext _context;

        public AddressRepository(HotelContext context)
        {
            _context = context;
        }

        // 🔹 Tìm kiếm tỉnh/thành phố
        public async Task<object> SearchProvincesAsync(SearchRequestDto searchRequestDto)
        {
            var provinces = await _context.Provinces
                .Where(p => string.IsNullOrWhiteSpace(searchRequestDto.SearchQuery) ||
                            p.Name.ToLower().Contains(searchRequestDto.SearchQuery.ToLower()) ||
                            p.Code.ToLower().Contains(searchRequestDto.SearchQuery.ToLower()) ||
                            p.Name.ToLower().Contains(searchRequestDto.SearchQuery.ToLower())
                           )
                .OrderBy(p => p.Name)
                .Select(p => new
                {
                    Type = "Province",
                    Code = p.Code,
                    Name = p.Name,
                    FullName = p.FullName
                })
                .Cast<object>()
                .ToListAsync();
            return new { result = provinces, success = true };

        }

        // 🔹 Tìm kiếm quận/huyện
        public async Task<object> GetDistrictsByProvinceAsync(SearchRequestDto searchRequestDto)
        {
            var districts = await _context.Districts
                .Where(d => d.Province.Code == searchRequestDto.Code &&
                            (string.IsNullOrEmpty(searchRequestDto.SearchQuery) || d.FullName.Contains(searchRequestDto.SearchQuery)))
                .Select(d => new
                {
                    Type = "District",
                    Code = d.Code,
                    Name = d.Name,
                    FullName = d.FullName
                })
                .ToListAsync();

            return new { result = districts, success = true };
        }

        // 🔹 Tìm kiếm phường/xã
        public async Task<object> GetWardsByDistrictAsync(SearchRequestDto searchRequestDto)
        {
            var wards = await _context.Wards
                .Where(d => d.District.Code == searchRequestDto.Code &&
                            (string.IsNullOrEmpty(searchRequestDto.SearchQuery) || d.FullName.Contains(searchRequestDto.SearchQuery)))
                .Select(w => new
                {
                    Type = "Ward",
                    Code = w.Code,
                    Name = w.Name,
                    FullName = w.FullName,
                    District = w.District.Name,
                    Province = w.District.Province.Name
                })
                .ToListAsync();

            return new { result = wards, success =true };
        }
        public async Task<IEnumerable<LocationSuggestionDto>> SearchLocationsAsync(string keyword)
        {
            keyword = keyword.ToLower();
            var hotels = await _context.Hotels
                .Where(h => h.Name.ToLower().Contains(keyword))
                .Select(h => new LocationSuggestionDto
                {
                    Type = "Hotel",
                    Code = h.Id,
                    Name = h.Name,
                    FullNameEn = "Hotel"
                }).ToListAsync();

            var wards = await _context.Wards
                .Where(w => w.Name.ToLower().Contains(keyword) && _context.Hotels.Any(h => h.WardCode == w.Code))
                .Join(_context.Districts, w => w.DistrictCode, d => d.Code, (w, d) => new { w, d })
                .Join(_context.Provinces, wd => wd.d.ProvinceCode, p => p.Code, (wd, p) => new { wd.w, wd.d, p })
                .Join(_context.AdministrativeUnits, wd_p => wd_p.w.AdministrativeUnitId, au => au.Id, (wd_p, au) => new LocationSuggestionDto
                {
                    Type = "Ward",
                    FullNameEn = au.FullNameEn,
                    Code = wd_p.w.Code,
                    Name = wd_p.w.Name,
                    Province = wd_p.p.Name,
                    District = wd_p.d.Name,
                    Ward = wd_p.w.Name,
                    HotelCount = _context.Hotels.Count(h => h.WardCode == wd_p.w.Code)
                })
                .ToListAsync();

            var districts = await _context.Districts
                .Where(d => d.Name.ToLower().Contains(keyword) && _context.Hotels.Any(h => h.DistrictCode == d.Code))
                .Join(_context.Provinces, d => d.ProvinceCode, p => p.Code, (d, p) => new { d, p })
                .Join(_context.AdministrativeUnits, dp => dp.d.AdministrativeUnitId, au => au.Id, (dp, au) => new LocationSuggestionDto
                {
                    Type = "District",
                    FullNameEn = au.FullNameEn,
                    Code = dp.d.Code,
                    Name = dp.d.Name,
                    Province = dp.p.Name,
                    District = dp.d.Name,
                    HotelCount = _context.Hotels.Count(h => h.DistrictCode == dp.d.Code)
                })
                .ToListAsync();

            var provinces = await _context.Provinces
                .Where(p => p.Name.ToLower().Contains(keyword) && _context.Hotels.Any(h => h.ProvinceCode == p.Code))
                .Join(_context.AdministrativeUnits, p => p.AdministrativeUnitId, au => au.Id, (p, au) => new LocationSuggestionDto
                {
                    Type = "Province",
                    FullNameEn = au.FullNameEn,
                    Code = p.Code,
                    Name = p.Name,
                    Province = p.Name,
                    HotelCount = _context.Hotels.Count(h => h.ProvinceCode == p.Code)
                })
                .ToListAsync();

            return hotels.Concat(wards).Concat(districts).Concat(provinces).ToList();
        }



    }
}
