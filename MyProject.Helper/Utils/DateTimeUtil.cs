namespace MyProject.Helper.Utils
{
    public static class DateTimeUtil
    {
        public static long GetTimestampSecond()
        {
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return timestamp;
        }
    }
}
