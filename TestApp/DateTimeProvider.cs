using System;

namespace TestApp
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime DateTimeUtcNow => DateTime.UtcNow;
    }
}
