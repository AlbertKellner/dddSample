using System;
using DDD.Sample.Foundation;

namespace DDD.Sample.Entityes
{
    /// <summary>
    /// Produto
    /// </summary>
    public class Product : Entity
    {
        internal Product() { }

        /// <summary>
        /// Código do Produto 
        /// </summary>
        public string CodeCatalog { get; set; }
        
        /// <summary>
        /// Nome
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Dados da Ultima compra
        /// </summary>
        public Lazy<Purchase> LastPurchase { get; set; }

        /// <summary>
        /// Informa uma compra no fornecedor indicado
        /// </summary>
        public Purchase PurchaseFrom(Supplier supplier, decimal price, decimal quantity)
        {
            var purchase = new Purchase {
                Product = this,
                Supplier = supplier,
                Price = price,
                Quantity = quantity 
            };
            LastPurchase = purchase.AsLazy();

            return purchase;
        }
    }
}
