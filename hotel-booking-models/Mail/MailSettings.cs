﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_models.Mail
{
    public class MailSettings
    {
        public string FromMail {  get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host {  get; set; }
        public int Port { get; set; }
    }
}
