﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDShop.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string NumberPhone { get; set; }
        public string Address { get; set; }
    }
}
