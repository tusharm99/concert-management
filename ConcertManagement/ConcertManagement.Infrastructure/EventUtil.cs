namespace ConcertManagement.Infrastructure
{
    public class EventUtil
    {
        public static string GenerateCode(string context, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var str = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return string.Concat(context, "-", str);
        }
    }
}
