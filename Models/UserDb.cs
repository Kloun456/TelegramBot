namespace CoffeBot.Models
{
    public class UserDb
    {
        public long Id { get; set; }
        public required string Name { get; set; }

        public bool IsAdmin { get; set; }
    }
}
