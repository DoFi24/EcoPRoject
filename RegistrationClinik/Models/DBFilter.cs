namespace RegistrationClinik.Models
{
    public class DBFilter
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}
