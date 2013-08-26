using System;

namespace DDD.Sample.Entityes
{
    /// <summary>
    /// Produto
    /// </summary>
    public class ProductNotFound : Exception 
    {
        public ProductNotFound(string codeCatalog)
            : base(string.Format("Product code {0} not found.", codeCatalog))
        {
            
        }

    }
}
