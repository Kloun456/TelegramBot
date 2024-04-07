namespace CoffeBot.Models
{
    public class Cup
    {
        public Guid Id { get; set; }

        public required UserDb UserDb { get; set; }
        public int CountCups { get; set; }

    }
}
