using System;
using DDD.Sample.Foundation;
using DDD.Sample.Entityes;

namespace DDD.Sample.Factory
{
    /// <summary>
    /// Criação de entidades
    /// </summary>
    public static class Create
    {
        /// <summary>
        /// Produto
        /// </summary>
        public static Product Product(string codeCatalog, string name)
        {
            using (var products = new Repository<Product>())
            {
                if (products.AnyByCode(codeCatalog))
                    throw new Exception(string.Format("Already a product with the code {0}", codeCatalog));

                var product = new Product
                                  {
                                      CodeCatalog = codeCatalog,
                                      Name = name
                                  };

                return product;
            }
        }
    }
}
