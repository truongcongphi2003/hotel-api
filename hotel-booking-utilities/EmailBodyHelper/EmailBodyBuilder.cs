﻿using hotel_booking_models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace hotel_booking_utilities.EmailBodyHelper
{
    public class EmailBodyBuilder
    {
        public static async Task<string> GetEmailBody(AppUser user, List<string> userRole, string emailTempPath, string linkName, string token, string controllerName)
        {
            var link = string.Empty;
            TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
            var userName = textInfo.ToTitleCase(user.FirstName);
            
            foreach (var role in userRole)
            {
                if (role == UserRoles.Admin || role == UserRoles.HotelManager)
                {
                    //link = _url.Action(linkName, controllerName, new { user.Email, token }, scheme);
                    link = $"http://localhost:3000/{linkName}?email={user.Email}&token={token}";
                }
                else
                {
                    link = $"https://localhost:7256/{linkName}?email={user.Email}&token={token}";
                }
            }

            var temp = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), emailTempPath));
            var newTemp = temp.Replace("**link**", link);
            var emailBody = newTemp.Replace("**User**", userName);
            return emailBody;
        }
        public static async Task<string> GetEmailBody(string emailTempPath, string token, string email)
        {
            var link = $"https://localhost:7256/Manager/validate-email?email={email}&token={token}";
            var temp = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), emailTempPath));
            var emailBody = temp.Replace("**link**", link);
            return emailBody;
        }
    }
}
