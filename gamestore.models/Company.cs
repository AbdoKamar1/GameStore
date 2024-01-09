﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamestore.models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? StreetName { get; set; }
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
    }
}