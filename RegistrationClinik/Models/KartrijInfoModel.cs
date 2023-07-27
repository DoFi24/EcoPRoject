using System;

namespace RegistrationClinik.Models
{
    public class KartrijInfoModel
    {
        public int Id { get; set; }
        public int KarId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
