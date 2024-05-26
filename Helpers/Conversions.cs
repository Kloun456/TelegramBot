namespace CoffeBot.Helpers
{
    public class Conversions
    {
        public static Guid LongToGuid(long value)
        {
            byte[] guidData = new byte[16];
            Array.Copy(BitConverter.GetBytes(value), guidData, 8);
            return new Guid(guidData);
        }
    }
}
