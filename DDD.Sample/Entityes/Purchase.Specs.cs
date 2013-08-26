using System.Linq;
using DDD.Sample.Foundation;

namespace DDD.Sample.Entityes
{
    /// <summary>
    /// Obtem as compras do produto indicado
    /// </summary>
    public class GetPurchaseByProduct : Specification<Purchase>
    {
        public GetPurchaseByProduct(string productCodeCatalog)
        {
            SatisfiedBy = p => p.ProductCode == productCodeCatalog;
        }
    }


    /// <summary>
    /// Helper para repositórios de dados de compra
    /// </summary>
    public static class PurchaseHelper
    {
        /// <summary>
        /// Obtém dados de compra
        /// </summary>
        public static IQueryable<Purchase> GetPurchasesByProduct(this Repository<Purchase> repository, Product product)
        {
            return repository.GetFrom(new GetPurchaseByProduct(product.CodeCatalog));
        }

        /// <summary>
        /// Obtém dados da ultima compra realizada
        /// </summary>
        public static Purchase GetLastPurchasesByProduct(this Repository<Purchase> repository, Product product)
        {
            return repository.GetFrom(new GetPurchaseByProduct(product.CodeCatalog))
                .OrderByDescending(p => p.ModifiedAt)
                .FirstOrDefault();


        }

    }
}
