﻿using DDD.Sample.Foundation;

namespace DDD.Sample.Entityes
{
    /// <summary>
    /// Obtém o produto pelo código
    /// </summary>
    public class ByCodeCatalog : Specification<Product>
    {
        /// <summary>
        /// Obtém o produto pelo código
        /// </summary>
        public ByCodeCatalog(string codeCatalog)
        {
            SatisfiedBy = product => product.CodeCatalog == codeCatalog;
        }
    }

    /// <summary>
    /// Helper para repositórios de produtos
    /// </summary>
    public static class ProductHelper
    {
        /// <summary>
        /// Retorna verdadeiro se existir algum produto na persistecia com o código indicado
        /// </summary>
        public static bool AnyByCode(this Repository<Product> repository, string codeCatalog)
        {
            return repository.Any(new ByCodeCatalog(codeCatalog));
        }

        /// <summary>
        /// Obtém o produto pelo seu código de catalogo
        /// </summary>
        public static Product GetByCode(this Repository<Product> repository, string codeCatalog)
        {
            return repository.Get(new ByCodeCatalog(codeCatalog));
        }

    }
}
