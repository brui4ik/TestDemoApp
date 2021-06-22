using System;

namespace TestApp
{
    public interface IDateTimeProvider
    {
        public DateTime DateTimeUtcNow { get; }
    }
}
