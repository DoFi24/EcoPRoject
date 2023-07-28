namespace RegistrationClinik.Models
{
    public class DBKartrij
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Srok { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
