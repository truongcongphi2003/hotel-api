using hotel_booking_data.Contexts;
using hotel_booking_models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace be.Data.Seeder
{
    public class HotelSeeder
    {
        public static async Task SeedData(HotelContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var baseDir = Directory.GetCurrentDirectory();
            await context.Database.EnsureCreatedAsync();

            if (!context.Users.Any())
            {
                List<string> roles = new List<string> { "Admin","Manager","Customer"};
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
                }

                var user = new AppUser
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Hotel",
                    LastName = "Admin",
                    UserName = "PHI",
                    Email = "admin@gmail.com",
                    PhoneNumber = "1234567890",
                    Gender = true,
                    DateOfBirth = new DateTime(2003, 1, 2),
                    IsActive = true,
                    PublicId = Guid.NewGuid().ToString(),
                    Avatar = "http://placehold.it/32x32",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
                user.EmailConfirmed = true;
                await userManager.CreateAsync(user,"Admin@123");
                await userManager.AddToRoleAsync(user, "Admin");
                
                //var path = File.ReadAllText(FilePath(baseDir, "Json/user.json"));

                //var hotelUsers = JsonConvert.DeserializeObject<List<AppUser>>(path);
                //for (int i = 0; i < hotelUsers.Count; i++)
                //{
                //    hotelUsers[i].EmailConfirmed = true;
                //    await userManager.CreateAsync(hotelUsers[i], "Admin@123");
                //    if(i < 5)
                //    {
                //        await userManager.AddToRoleAsync(hotelUsers[i], "Manager");
                //        continue;
                //    }
                //    await userManager.AddToRoleAsync(hotelUsers[i],"Customer");
                //}
            }
            //if (!context.Amenities.Any())
            //{
            //    var path = File.ReadAllText(FilePath(baseDir, "Json/amenities.json"));

            //    var amenities = JsonConvert.DeserializeObject<List<Amenity>>(path);
            //    await context.Amenities.AddRangeAsync(amenities);
            //}

            //// Bookings and Payment
            //if (!context.Bookings.Any())
            //{
            //    var path = File.ReadAllText(FilePath(baseDir, "Json/bookings.json"));

            //    var bookings = JsonConvert.DeserializeObject<List<Booking>>(path);
            //    await context.Bookings.AddRangeAsync(bookings);
            //}

            //// Hotels, roomtypes n rooms
            //if (!context.Hotels.Any())
            //{
            //    var path = File.ReadAllText(FilePath(baseDir, "Json/hotel.json"));

            //    var hotels = JsonConvert.DeserializeObject<List<Hotel>>(path);
            //    await context.Hotels.AddRangeAsync(hotels);
            //}

            //// Wishlist items
            //if (!context.WishLists.Any())
            //{
            //    var path = File.ReadAllText(FilePath(baseDir, "Json/wishlists.json"));

            //    var wishList = JsonConvert.DeserializeObject<List<WishList>>(path);
            //    await context.WishLists.AddRangeAsync(wishList);
            //}

            //if (!context.AdministrativeRegions.Any())
            //{
            //    var path = File.ReadAllText(FilePath(baseDir, "Json/AdministrativeRegions.json"));

            //    var administrativeRegions = JsonConvert.DeserializeObject<List<AdministrativeRegion>>(path);
            //    await context.AdministrativeRegions.AddRangeAsync(administrativeRegions);
            //}
            //if (!context.AdministrativeUnits.Any())
            //{
            //    var path = File.ReadAllText(FilePath(baseDir, "Json/AdministrativeUnits.json"));
            //    var administrativeUnits = JsonConvert.DeserializeObject<List<AdministrativeUnit>>(path);
            //    await context.AdministrativeUnits.AddRangeAsync(administrativeUnits);
            //}
            //if (!context.Provinces.Any())
            //{
            //    var path = File.ReadAllText(FilePath(baseDir, "Json/Provinces.json"));
            //    var provinces = JsonConvert.DeserializeObject<List<Province>>(path);
            //    await context.Provinces.AddRangeAsync(provinces);
            //}
            //if (!context.Districts.Any())
            //{
            //    var path = File.ReadAllText(FilePath(baseDir, "Json/Districts.json"));
            //    var districts = JsonConvert.DeserializeObject<List<District>>(path);
            //    await context.Districts.AddRangeAsync(districts);
            //}
            //if (!context.Wards.Any())
            //{
            //    var path = File.ReadAllText(FilePath(baseDir, "Json/Wards.json"));
            //    var wards = JsonConvert.DeserializeObject<List<Ward>>(path);
            //    await context.Wards.AddRangeAsync(wards);
            //}

            await context.SaveChangesAsync();

        }
        static string FilePath(string folderName, string fileName)
        {
            return Path.Combine(folderName, fileName);
        }
    }
}
