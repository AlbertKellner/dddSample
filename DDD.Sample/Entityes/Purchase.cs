namespace DDD.Sample.Entityes
{
    /// <summary>
    /// Dados de compras
    /// </summary>
    public class Purchase : Entity
    {
        /// <summary>
        /// Produto
        /// </summary>
        public Product Product
        {
            get
            {
                return _product;
            }
            set
            {
                _product = value;
                ProductCode = _product.CodeCatalog;
            }
        } private Product _product;
        
        /// <summary>
        /// Fornecedor
        /// </summary>
        public Supplier Supplier { get; set; }

        /// <summary>
        /// Preço de Compra
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Quantidade comprada
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Código do Produto
        /// </summary>
        internal string ProductCode { get; set; }
    }
}
