using System.Linq;
using DDD.Sample.Foundation;

namespace DDD.Sample.Entityes
{
    /// <summary>
    /// Obtem as compras do produto indicado
    /// </summary>
    public class GetAllWarehouseWithProduct : Specification<ProductInWarehouse>
    {
        public GetAllWarehouseWithProduct(string productCodeCatalog)
        {
            SatisfiedBy = p => p.ProductCode == productCodeCatalog;
        }
    }

    /// <summary>
    /// Obtem as compras do produto indicado
    /// </summary>
    public class GetProductInWarehouse : Specification<ProductInWarehouse>
    {
        public GetProductInWarehouse(string productCodeCatalog, string warehouseCode)
        {
            SatisfiedBy = p => p.ProductCode == productCodeCatalog && p.WarehouseCode == warehouseCode;
        }
    }
    
    /// <summary>
    /// Helper para Produto no depósito
    /// </summary>
    public static class ProductInWarehouseHelper 
    {
        /// <summary>
        /// Obtém todos as informações de produtos dos depositos
        /// </summary>
        public static IQueryable<ProductInWarehouse> GetProducts(this Repository<ProductInWarehouse> repository, Product product)
        {
            return repository.GetFrom(new GetAllWarehouseWithProduct(product.CodeCatalog));
        }

        /// <summary>
        /// Obtém todos as informações de produtos dos depositos
        /// </summary>
        public static ProductInWarehouse GetProductsInWarehouse(this Repository<ProductInWarehouse> repository, 
            Product product, Warehouse warehouse)
        {
            ProductInWarehouse productInWarehouse = repository.Get(new GetProductInWarehouse(product.CodeCatalog, warehouse.Code));
            productInWarehouse.Product = product;
            productInWarehouse.Warehouse = warehouse;
            return productInWarehouse;
        }
    }
}
