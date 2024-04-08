namespace CoffeBot.Models
{
    public class UserDb
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        public bool IsAdmin { get; set; }

        public Cup Cup { get; set; }
    }
}
