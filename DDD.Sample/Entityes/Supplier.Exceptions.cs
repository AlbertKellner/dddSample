using System;

namespace DDD.Sample.Entityes
{
    /// <summary>
    /// Fornecedor
    /// </summary>
    public class SupplierNotFound : Exception
    {
        public SupplierNotFound(string code) 
            :base(string.Format("Supplier code {0} not found.", code))
        { }

    }
}
