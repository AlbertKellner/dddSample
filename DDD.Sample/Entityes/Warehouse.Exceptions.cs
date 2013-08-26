using System;

namespace DDD.Sample.Entityes
{
    /// <summary>
    /// Depósito não enconttrado
    /// </summary>
    public class WarehouseNotFound : Exception
    {
        public WarehouseNotFound(string code)
            : base(string.Format("Warehouse {0} not found", code))
        {
            
        }
    }

    /// <summary>
    /// Depósito não enconttrado
    /// </summary>
    public class DefaultWarehouseNotFound : Exception
    {
        public DefaultWarehouseNotFound()
            : base(string.Format("Default Warehouse not found, configure system parameters with code {0}.", SystemParameters.DefaultWarehouseCode))
        {

        }
    }
}
