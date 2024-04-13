using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeBot.Models
{
    public class Cup
    {
        [Key]
        [ForeignKey("UserDb")]
        public Guid Id { get; set; }

        public UserDb UserDb { get; set; }
        public int CountCups { get; set; }

    }
}
