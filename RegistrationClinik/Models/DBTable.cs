using System;
using System.Collections.Generic;

namespace RegistrationClinik.Models
{
    public class DBTable
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Adres { get; set; }
        public string? PhoneNumber { get; set; }
        public int ModelId { get; set; }
        public int IsActive { get; set; } 
    }
}
