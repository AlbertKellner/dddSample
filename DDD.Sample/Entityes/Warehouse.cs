using System;
using System.Collections.Generic;

namespace DDD.Sample.Entityes
{
    /// <summary>
    /// Depósito
    /// </summary>
    public class Warehouse : Entity
    {
        /// <summary>
        /// Identificação do depósito
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Produtos estocados
        /// </summary>
        public ICollection<ProductInWarehouse> ProductsInWarehouse { get; set; }

        /// <summary>
        /// Registra a entrada do produto no depósito pela primeira compra
        /// </summary>
        public ProductInWarehouse RegisterNewProductStock(Purchase purchase)
        {
            var productInWarehouse = new ProductInWarehouse
                                         {
                                             Warehouse = this,
                                             Product = purchase.Product,
                                             Total = purchase.Quantity,
                                             AveragePrice = purchase.Price
                                         };
            return productInWarehouse;
        }

        /// <summary>
        /// Atualiza os dados de depósito por nova compra efetuada
        /// </summary>
        public void UpdateStock(ProductInWarehouse productInWarehouse, Purchase purchase)
        {
            var quantity = productInWarehouse.Total + purchase.Quantity;
            var total = (productInWarehouse.AveragePrice * productInWarehouse.Total) + (purchase.Quantity * purchase.Price);
            var average = Decimal.Round(total / quantity, 4);

            productInWarehouse.Total = quantity;
            productInWarehouse.AveragePrice = average;
        }
    }
}
