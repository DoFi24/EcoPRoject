using System;

namespace RegistrationClinik.Models
{
    public class DBKartrigList
    {
        public int Id { get; set; }
        public int KartrijId { get; set; }
        public int TableId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
