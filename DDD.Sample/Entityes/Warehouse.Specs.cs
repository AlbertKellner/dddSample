using DDD.Sample.Foundation;

namespace DDD.Sample.Entityes
{
    /// <summary>
    /// Obtém o depósito padrão
    /// </summary>
    public class DefaultWarehouse : Specification<Warehouse>
    {
        /// <summary>
        ///  Obtém o depósito padrão
        /// </summary>
        public DefaultWarehouse()
        {
            using (var systemParameters = new Repository<SystemParameters>())
            {
                var defaultWarehouseCode = systemParameters.Get(new SystemParameterByCode(SystemParameters.DefaultWarehouseCode)).Value;
                SatisfiedBy = warehouse => warehouse.Code == defaultWarehouseCode;
            }
        }
    }

    /// <summary>
    /// Obtém o depósito por código
    /// </summary>
    public class WarehouseByCode : Specification<Warehouse>
    {
        public readonly string Code;

        /// <summary>
        ///  Obtém o depósito padrão
        /// </summary>
        public WarehouseByCode(string warehouseCode)
        {
            Code = warehouseCode; 
            SatisfiedBy = warehouse => warehouse.Code == Code;
        }
    }

    /// <summary>
    /// Helper para repositórios de depósitos
    /// </summary>
    public static class WarehouseHelper
    {
        /// <summary>
        /// Obter depósito padrão
        /// </summary>
        public static Warehouse GetDefaultWarehouse(this Repository<Warehouse> repository)
        {
            return repository.Get(new DefaultWarehouse());
        }

        /// <summary>
        /// Obter depósito por código
        /// </summary>
        public static Warehouse GetByCode(this Repository<Warehouse> repository, string warehouseCode)
        {
            return repository.Get(new WarehouseByCode(warehouseCode));
        }
    }
}
