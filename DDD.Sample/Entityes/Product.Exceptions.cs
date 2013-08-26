using System;

namespace DDD.Sample.Entityes
{
    /// <summary>
    /// Produto não encontrado  
    /// </summary>
    public class ProductNotFound : Exception 
    {
        public ProductNotFound(string codeCatalog)
            : base(string.Format("Product code {0} not found.", codeCatalog))
        {
            
        }
    }

    /// <summary>
    /// Produto com mesmo código
    /// </summary>
    public class ProductSameCode : Exception
    {
        public ProductSameCode(string codeCatalog)
            : base(string.Format("Already a product with the code {0}", codeCatalog))
        {

        }
    }

    /// <summary>
    /// Produto com mesmo nome
    /// </summary>
    public class ProductSameName : Exception
    {
        public ProductSameName(string name)
            : base(string.Format("Already a product with the name {0}", name))
        {

        }
    }
}
