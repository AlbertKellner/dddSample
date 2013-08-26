using System;

namespace DDD.Sample.Foundation
{
    public interface IClock
    {
        DateTime GetNow();
    }
}
