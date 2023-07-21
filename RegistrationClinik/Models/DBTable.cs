using System;

namespace RegistrationClinik.Models
{
    public class DBTable
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Adres { get; set; }
        public string? PhoneNumber { get; set; }
        public DBFilter Model { get; set; }
        public DateTime? StartTimer { get; set; }
        public DateTime? EndTimer { get; set; }
        public int IsActive { get; set; } 
    }
}
