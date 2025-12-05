using Microsoft.Extensions.Caching.Memory;

namespace EmployeeInformations.Business.Utility.Caching
{
    public class MemoryCacheManager
    {
        public MemoryCache Cache { get; } = new MemoryCache(

        new MemoryCacheOptions()
        {
            ExpirationScanFrequency = new TimeSpan(0, 0, 15, 0)
        });
    }
}
