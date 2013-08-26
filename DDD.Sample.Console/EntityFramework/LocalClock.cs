using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD.Sample.Foundation;

namespace DDD.Sample.Console.EntityFramework
{
    class LocalClock : IClock
    {
        public DateTime GetNow()
        {
            return DateTime.Now;
        }
    }
}
