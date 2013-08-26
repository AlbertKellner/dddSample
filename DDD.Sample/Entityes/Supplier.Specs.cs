using DDD.Sample.Foundation;

namespace DDD.Sample.Entityes
{
    /// <summary>
    /// Obtém o fornnecedor pelo código
    /// </summary>
    public class SupplierByCode : Specification<Supplier>
    {
        /// <summary>
        /// Código
        /// </summary>
        public readonly string Code;

        /// <summary>
        /// Fornnecedor pelo código
        /// </summary>
        public SupplierByCode(string code)
            : base(supplier => supplier.Code ==  code)
        {
            Code = code;
        }
    }

    /// <summary>
    /// Helper para repositórios de Forcenecores
    /// </summary>
    public static class SupplierHelper
    {
        /// <summary>
        /// Obtém o fornnecedor pelo código
        /// </summary>
        public static Supplier GetSupplierByCode(this Repository<Supplier> repository, string code)
        {
            return repository.Get(new SupplierByCode(code));
        }
    }
}
