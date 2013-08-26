namespace DDD.Sample.Entityes
{
    /// <summary>
    /// Informações do Produto no depósito
    /// </summary>
    public class ProductInWarehouse : Entity
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
        /// Depoósito
        /// </summary>
        public Warehouse Warehouse 
        {
            get
            {
                return _warehouse;
            }
            set
            {
                _warehouse = value;
                WarehouseCode = _warehouse.Code;
            }
        } private Warehouse _warehouse;
        
        /// <summary>
        /// Quantidade em estoque
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Preço Médio
        /// </summary>
        public decimal AveragePrice { get; set; }

        /// <summary>
        /// Código do Produto
        /// </summary>
        internal string ProductCode { get; set; }

        /// <summary>
        /// Código do Deposito
        /// </summary>
        internal string WarehouseCode { get; set; }
    }
}
