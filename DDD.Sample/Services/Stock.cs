using DDD.Sample.Entityes;
using DDD.Sample.Foundation;

namespace DDD.Sample.Services
{
    /// <summary>
    /// Informações de estoque e depósitos
    /// </summary>
    public class Stock
    {
        /// <summary>
        /// Retorna informações sobre o depósito de entrada de mercadorias padrão
        /// </summary>
        public Warehouse GetDefaultWarehouse()
        {
            using (var warehouses = new Repository<Warehouse>())
            {
                return warehouses.GetDefaultWarehouse();
            }
        }

        /// <summary>
        /// Retorna as informações do produto no estoque indicado
        /// </summary>
        public ProductInWarehouse GetProductInWarehouse(string productCode, string warehouseCode)
        {
            using (var products = new Repository<Product>()) 
            using (var warehouses = new Repository<Warehouse>())
            using (var warehousesProducts = new Repository<ProductInWarehouse>())
            {
                var product = products.GetByCode(productCode);
                if (product == null)
                    throw new ProductNotFound(productCode);
                var warehouse = warehouses.GetByCode(warehouseCode);
                if (warehouse == null)
                    throw new WarehouseNotFound(warehouseCode);

                var productInWarehouse = warehousesProducts.GetProductsInWarehouse(product, warehouse);
                return productInWarehouse;
            }
        }
    }
}
